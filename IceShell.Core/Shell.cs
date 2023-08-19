// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core;

using global::IceShell.Core;
using global::IceShell.Core.CLI;
using global::IceShell.Core.CLI.Languages;
using global::IceShell.Core.Commands;
using IceShell.Core.Commands;
using NexusKrop.IceCube;
using NexusKrop.IceCube.Settings;
using NexusKrop.IceShell.Core.Api;
using NexusKrop.IceShell.Core.CLI;
using NexusKrop.IceShell.Core.Commands.Complex;
using NexusKrop.IceShell.Core.Completion;
using NexusKrop.IceShell.Core.Completion.Cache;
using NexusKrop.IceShell.Core.Exceptions;
using NexusKrop.IceShell.Core.FileSystem;
using ReadLineReboot;
using System;
using System.Diagnostics;

/// <summary>
/// Represents the command-line interactive shell, the core of IceShell.
/// </summary>
public class Shell : IShell
{
    public const string DefaultPrompt = "%P%G ";

    private readonly CommandParser _parser = new();
    private readonly ShellSettings _settings;
    private readonly CommandDispatcher _dispatcher;

    private static readonly DirCache DIR_CACHE = new(Environment.CurrentDirectory);
    private static readonly string WORKINGDIR_EXECUTABLE_DELIMITER = ".\\";

    public string Prompt { get; set; }

    public ITerminal Terminal { get; set; } = new RealTerminal();

    public Shell() : this(new())
    {
    }

    public Shell(ShellSettings settings)
    {
        _settings = settings;
        Prompt = DefaultPrompt;
        _dispatcher = new(this);
    }

    public static CommandManager CommandManager { get; } = new();
    public static ModuleManager ModuleManager { get; } = new();

    /// <summary>
    /// Gets or sets whether all shells will exit after executing their next command, or
    /// evaluated their next user input.
    /// </summary>
    public static bool ExitShell { get; set; }

    public bool SupportsJump => false;

    /// <summary>
    /// Changes the current directory of this process.
    /// </summary>
    /// <param name="target">The target directory. Must be a system path.</param>
    public static void ChangeDirectory(string target)
    {
        DIR_CACHE.UpdateDirectory(target);
        Directory.SetCurrentDirectory(target);
    }

    public void Quit()
    {
        Shell.QuitStatic();
    }

    /// <summary>
    /// Tasks all shell instances to exit after executing their next (or last, if currently executing) command or user input.
    /// </summary>
    public static void QuitStatic()
    {
        ExitShell = true;
    }

    /// <summary>
    /// Executes an executable in the current folder, and if success, waits for it to exit.
    /// </summary>
    /// <param name="fileName">The name of the file.</param>
    /// <param name="args">The arguments.</param>
    /// <returns><see langword="true"/> if a valid executable was found; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="CommandFormatException">The specified file was not executable.</exception>
    public bool ExecuteOnDisk(string fileName, string[]? args)
    {
        var actual = PathSearcher.GetSystemExecutableName(Path.Combine(Environment.CurrentDirectory, fileName));

        if (!File.Exists(actual))
        {
            return false;
        }

        var startInfo = new ProcessStartInfo(actual)
        {
            WorkingDirectory = Environment.CurrentDirectory
        };

        args?.ForEach(startInfo.ArgumentList.Add);

        if (!FileUtil.IsExecutable(actual))
        {
            throw new CommandFormatException(string.Format(Languages.Get("shell_not_executable"), actual));
        }

        Process.Start(startInfo)?.WaitForExit();

        return true;
    }

    /// <summary>
    /// Executes an executable located in <c>PATH</c>.
    /// </summary>
    /// <param name="fileName">The name of the executable. Subdirectories are prohibited.</param>
    /// <param name="args">The arguments to pass to the executable.</param>
    /// <returns><see langword="true"/> if a valid executable was found; otherwise, <see langword="false"/>.</returns>
    public static bool ExecuteOnPath(string fileName, string[]? args)
    {
        var actual = PathSearcher.SearchExecutable(fileName);

        if (actual == null)
        {
            return false;
        }

        var startInfo = new ProcessStartInfo(fileName)
        {
            WorkingDirectory = Environment.CurrentDirectory
        };

        args?.ForEach(startInfo.ArgumentList.Add);

        Process.Start(startInfo)?.WaitForExit();

        return true;
    }

    /// <summary>
    /// Parses and then executes the specified user input.
    /// </summary>
    /// <param name="line">The input.</param>
    public int Execute(string line, ICommandExecutor? actualExecutor = null)
    {
        _parser.SetLine(line);
        var command = _parser.ReadString();

        try
        {
            string[]? args = null;

            if (string.IsNullOrWhiteSpace(command))
            {
                ConsoleOutput.PrintShellError(Languages.Get("shell_empty_command"));
                return 1;
            }

            // If starts with "dot limiter" (.\ etc) explicitly execute it in working dir
            if (command.StartsWith(WORKINGDIR_EXECUTABLE_DELIMITER))
            {
                _parser.ReadArgs(out args);

                if (!ExecuteOnDisk(command, args))
                {
                    ConsoleOutput.PrintShellError(Languages.InvalidFile(command));
                    return 1;
                }

                return 0;
            }

            var type = CommandManager.GetComplex(command);

            // If no such complex command either
            if (type == null)
            {
                // Reads the arguments out
                _parser.ReadArgs(out args);

                // Executes them on path
                if (!ExecuteOnPath(command, args))
                {
                    // If no such path file, say bad command
                    ConsoleOutput.PrintShellError(Languages.UnknownCommand(command));
                    return 1;
                }

                return 0;
            }

            var parsed = CommandDispatcher.Parse(command, _parser);
            return _dispatcher.Execute(parsed, actualExecutor ?? this);
        }
        catch (CommandFormatException ex)
        {
            ConsoleOutput.PrintShellError(string.Format("{0}: {1}", command, ex.Message));
        }
        catch (Exception ex)
        {
            ConsoleOutput.PrintShellError(ex.ToString());
        }

        // Fallback = fails
        return 1;
    }

    /// <summary>
    /// Runs the shell interactively.
    /// </summary>
    /// <returns>The exit code.</returns>
    public int RunInteractive()
    {
        ReadLine.HistoryEnabled = true;
        ReadLine.AutoCompletionHandler = new ShellCompletionHandler(CommandManager, DIR_CACHE);

        ModuleManager.LoadModules(Path.Combine(Path.GetDirectoryName(Environment.ProcessPath!)!, "modules"));
        ModuleManager.InitializeModules();

        // show date and time if allowed
        if (_settings.DisplayDateTimeOnStartup)
        {
            System.Console.WriteLine($"Current date is {DateTime.Now.ToShortDateString()}");
            System.Console.WriteLine($"Current time is {DateTime.Now.ToShortTimeString()}");
        }

        // Add an empty line afterward
        System.Console.WriteLine();

        while (!ExitShell)
        {
            var prompt = this.Prompt.Replace("%P", PathSearcher.SystemToShell(Environment.CurrentDirectory), true, null)
                .Replace("%G", ">", true, null)
                .Replace("%L", "<", true, null);

            // TODO reference the ReadLine code and use Terminal interface
            var input = ReadLine.Read(prompt);

            if (string.IsNullOrWhiteSpace(input))
            {
                continue;
            }

            Execute(input, null);
            Console.WriteLine();
        }

        return 0;
    }

    public void WriteLine()
    {
        Terminal.StandardOutput.WriteLine();
    }

    public void WriteLine(string? text)
    {
        Terminal.StandardOutput.WriteLine(text);
    }

    public void WriteLine(object? obj)
    {
        Terminal.StandardOutput.WriteLine(obj);
    }

    public void WriteLine(string text, params object[] args)
    {
        Terminal.StandardOutput.WriteLine(text, args);
    }

    public void Write(string text, params object[] args)
    {
        Terminal.StandardOutput.Write(text, args);
    }

    public void Write(object? obj)
    {
        Terminal.StandardOutput.Write(obj);
    }

    public void Write(string? text)
    {
        Terminal.StandardOutput.Write(text);
    }

    public void Jump(string label)
    {
        throw new NotSupportedException();
    }
}
