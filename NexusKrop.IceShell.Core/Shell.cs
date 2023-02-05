// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core;

using NexusKrop.IceCube;
using NexusKrop.IceShell.Core.Api;
using NexusKrop.IceShell.Core.CLI;
using NexusKrop.IceShell.Core.Commands;
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
public class Shell
{
    private readonly CommandParser _parser = new();

    private static readonly DirCache DIR_CACHE = new(Environment.CurrentDirectory);
    private static readonly string WORKINGDIR_EXECUTABLE_DELIMITER = ".\\";

    public static CommandManager CommandManager { get; } = new();
    public static ModuleManager ModuleManager { get; } = new();

    /// <summary>
    /// Gets or sets whether all shells will exit after executing their next command, or
    /// evaluated their next user input.
    /// </summary>
    public static bool ExitShell { get; set; }

    /// <summary>
    /// Changes the current directory of this process.
    /// </summary>
    /// <param name="target">The target directory. Must be a system path.</param>
    public static void ChangeDirectory(string target)
    {
        Directory.SetCurrentDirectory(target);
        DIR_CACHE.UpdateDirectory(target);
    }

    /// <summary>
    /// Tasks all shell instances to exit after executing their next (or last, if currently executing) command or user input.
    /// </summary>
    public static void Quit()
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

        args?.Iterate(x => startInfo.ArgumentList.Add(x));

        if (!FileUtil.IsExecutable(actual))
        {
            throw new CommandFormatException(Messages.FileNotExecutable);
        }

        Process.Start(startInfo)?.WaitForExit();

        return true;
    }

    /// <summary>
    /// Executes an executable located in <c>PATH</c>.
    /// </summary>
    /// <param name="fileName">The name of the executable. Subdirectorys are prohibited.</param>
    /// <param name="args">The arguments to pass to the executable.</param>
    /// <returns><see langword="true"/> if a valid executable was found; otherwise, <see langword="false"/>.</returns>
    public bool ExecuteOnPath(string fileName, string[]? args)
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

        args?.Iterate(x => startInfo.ArgumentList.Add(x));

        Process.Start(startInfo)?.WaitForExit();

        return true;
    }

    /// <summary>
    /// Parses and then executes the specified user input.
    /// </summary>
    /// <param name="input">The input.</param>
    public void Execute(string input)
    {
        _parser.SetLine(input);
        var command = _parser.ReadString();

        try
        {
            string[]? args = null;

            if (string.IsNullOrWhiteSpace(command))
            {
                ConsoleOutput.PrintShellError(Messages.EmptyCommand);
                return;
            }

            // If starts with "dot limiter" (.\ etc) explicitly execute it in working dir
            if (command.StartsWith(WORKINGDIR_EXECUTABLE_DELIMITER))
            {
                _parser.ReadArgs(out args);

                if (!ExecuteOnDisk(command, args))
                {
                    ConsoleOutput.PrintShellError(Messages.BadFile);
                }
                else
                {
                    // Add a new blank line
                    Console.WriteLine();
                }

                return;
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
                    ConsoleOutput.PrintShellError(Messages.BadCommand);
                }
                else
                {
                    // Add a new blank line
                    Console.WriteLine();
                }

                return;
            }

            var instance = Activator.CreateInstance(type);

            if (instance is IComplexCommand ixcmd)
            {
                var arg = new ComplexArgument(_parser);

                ixcmd.Define(arg);
                ixcmd.Execute(arg.Parse());
                Console.WriteLine();
            }
            else
            {
                ConsoleOutput.PrintShellError(Messages.BadCommand);
            }
        }
        catch (CommandFormatException ex)
        {
            ConsoleOutput.PrintShellError(string.Format("{0}: {1}", command, ex.Message));
        }
        catch (Exception ex)
        {
            ConsoleOutput.PrintShellError(ex.ToString());
        }
    }

    /// <summary>
    /// Runs the shell interactively.
    /// </summary>
    /// <returns>The exit code.</returns>
    public int RunInteractive()
    {
        Console.WriteLine();
        ReadLine.HistoryEnabled = true;
        ReadLine.AutoCompletionHandler = new ShellCompletionHandler(CommandManager, DIR_CACHE);

        ModuleManager.LoadModules(Path.Combine(Path.GetDirectoryName(Environment.ProcessPath!)!, "modules"));

        while (!ExitShell)
        {
            var input = ReadLine.Read(string.Format("{0}> ", Environment.CurrentDirectory));

            if (string.IsNullOrWhiteSpace(input))
            {
                continue;
            }

            Execute(input);
        }

        return 0;
    }
}
