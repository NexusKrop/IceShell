// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core;

using global::IceShell.Core;
using global::IceShell.Core.Api;
using global::IceShell.Core.CLI.Languages;
using global::IceShell.Core.Commands;
using global::IceShell.Core.Exceptions;
using global::IceShell.Settings;
using IceShell.Core.Commands;
using NexusKrop.IceCube;
using NexusKrop.IceShell.Core.CLI;
using NexusKrop.IceShell.Core.Completion;
using NexusKrop.IceShell.Core.Completion.Cache;
using NexusKrop.IceShell.Core.FileSystem;
using ReadLineReboot;
using System;
using System.Diagnostics;
using System.Reflection;

/// <summary>
/// Represents the command-line interactive shell, the core of IceShell.
/// </summary>
public class Shell : IShell
{
    /// <summary>
    /// Gets the version of the shell interpreter application.
    /// </summary>
#if DEBUG
    public static readonly string AppVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion ?? "unknown";
#else
    public static readonly string AppVersion = FileVersionInfo.GetVersionInfo(Environment.ProcessPath ?? "iceshell.exe").ProductVersion ?? "unknown";
#endif

    private bool _exit;

    /// <summary>
    /// The default prompt string of the interactive shell.
    /// </summary>
    public const string DefaultPrompt = "%P%G ";

    private readonly ShellSettings _settings;

    private static readonly DirCache DIR_CACHE = new(Environment.CurrentDirectory);

    /// <inheritdoc />
    public string Prompt { get; set; }

    /// <inheritdoc />
    public CommandDispatcher Dispatcher { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Shell"/> class, with the default settings.
    /// </summary>
    public Shell() : this(new())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Shell"/> class.
    /// </summary>
    /// <param name="settings">The settings.</param>
    public Shell(ShellSettings settings)
    {
        _settings = settings;
        Prompt = DefaultPrompt;
        Dispatcher = new(this);
    }

    // TODO replace those global stuff with interfaces and instance properties

    /// <summary>
    /// Gets the global <see cref="CommandManager"/>.
    /// </summary>
    public static CommandManager CommandManager { get; } = new();

    /// <summary>
    /// Gets the global <see cref="ModuleManager"/>.
    /// </summary>
    public static ModuleManager ModuleManager { get; } = new();

    /// <inheritdoc />
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

    /// <summary>
    /// Quits this instance.
    /// </summary>
    public void Quit()
    {
        _exit = true;
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
    /// <param name="actualExecutor">The executor to pass to commands (or have this instance to act on behalf of). If <see langword="null"/>, the Shell will run commands on its own behalf.</param>
    public int Execute(string line, ICommandExecutor? actualExecutor = null)
    {
        try
        {
            var batchLine = CommandDispatcher.ParseLine(line);

            Dispatcher.Execute(batchLine, this);
        }
        catch (CommandFormatException ex)
        {
            ConsoleOutput.PrintShellError(string.Format("{0}", ex.Message));
        }
        catch (Exception ex)
        {
            ConsoleOutput.PrintShellError(ex.ToString());
        }

        // Fallback = fails
        return 1;
    }

    /// <inheritdoc />
    public int Execute(BatchLineCompound compound, ICommandExecutor? actualExecutor = null)
    {
        return Dispatcher.Execute(compound, actualExecutor ?? this);
    }

    /// <summary>
    /// Starts the interactive shell process and logic on the current thread, and returns after the current
    /// shell instance have exited.
    /// </summary>
    /// <returns>The exit code.</returns>
    public int RunInteractive()
    {
        ReadLine.HistoryEnabled = true;
        ReadLine.AutoCompletionHandler = new ShellCompletionHandler(CommandManager, DIR_CACHE);

        ModuleManager.LoadModules(Path.Combine(Path.GetDirectoryName(Environment.ProcessPath!)!, "modules"));
        ModuleManager.InitializeModules();

        // show date and time if allowed
        if (_settings.DisplayDateTimeOnStartUp)
        {
            System.Console.WriteLine($"Current date is {DateTime.Now.ToShortDateString()}");
            System.Console.WriteLine($"Current time is {DateTime.Now.ToShortTimeString()}");
        }

        if (_settings.DisplayShellInfoOnStartUp)
        {
            Console.WriteLine(Languages.Get("ver_line_0"), AppVersion);
        }

        // Add an empty line afterward
        System.Console.WriteLine();

        while (!_exit)
        {
            var prompt = this.Prompt.Replace("%P", PathSearcher.SystemToShell(Environment.CurrentDirectory), true, null)
                .Replace("%G", ">", true, null)
                .Replace("%L", "<", true, null);

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

    /// <summary>
    /// Throws <see cref="NotSupportedException"/> as interactive shells should never support skipping through lines.
    /// </summary>
    /// <param name="label">The label to skip to. Ignored.</param>
    /// <exception cref="NotSupportedException">The action is not supported.</exception>
    public void Jump(string label)
    {
        throw new NotSupportedException();
    }
}
