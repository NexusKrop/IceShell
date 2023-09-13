// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Core.Commands;

using IceShell.Core.CLI.Languages;
using IceShell.Core.Exceptions;
using IceShell.Parsing;
using NexusKrop.IceCube;
using NexusKrop.IceShell.Core;
using NexusKrop.IceShell.Core.CLI;
using NexusKrop.IceShell.Core.Commands.Complex;
using NexusKrop.IceShell.Core.FileSystem;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NewCommandParser = Parsing.CommandParser;
using OldCommandParser = NexusKrop.IceShell.Core.CommandParser;

/// <summary>
/// Provides a service to shells that parses and executes commands on its behalf.
/// </summary>
public class CommandDispatcher
{
    private readonly IShell _shell;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandDispatcher"/> class.
    /// </summary>
    /// <param name="shell">The shell to act on behalf of.</param>
    public CommandDispatcher(IShell shell)
    {
        _shell = shell;
    }

    /// <summary>
    /// Parses a single line of command or a call to external program.
    /// </summary>
    /// <param name="line">The line to parse.</param>
    /// <returns>The parsed BatchLine.</returns>
    public static BatchLine ParseLine(string line)
    {
        var statements = new LineParser().ParseLine(line);
        // If there is no such command, the batch line is parsed as calling for an executable.
        var cmdName = statements[0].Content;
        var definition = Shell.CommandManager.GetDefinition(cmdName);

        // If user specifies '.' or directory separator, this is a hard relative path.
        if (cmdName.StartsWith('.') || cmdName.StartsWith(Path.DirectorySeparatorChar) || cmdName.StartsWith('\\')
            || definition == null)
        {
            return new BatchLine(statements);
        }

        var parsed = NewCommandParser.ParseSingleCommand(statements);

        var argument = new CommandArgument(parsed, definition.Definition);
        var result = argument.Parse();

        var cmdInfo = new ParsedCommand(result, definition);
        return new BatchLine(cmdInfo, cmdName);
    }

    /// <summary>
    /// Executes the specified batch line.
    /// </summary>
    /// <param name="line">The line to execute.</param>
    /// <param name="executor">The executor to act on behalf of.</param>
    /// <returns>The exit code of the command or process; if failed to start external command, returns <c>-255</c>.</returns>
    /// <exception cref="ArgumentException">The specified <see cref="BatchLine"/> is invalid.</exception>
    /// <exception cref="CommandFormatException">The specified command was not found.</exception>
    public int Execute(BatchLine line, ICommandExecutor executor)
    {
        if (string.IsNullOrWhiteSpace(line.Name))
        {
            return 0;
        }

        if (line.IsCommand && line.Command != null)
        {
            return Execute(line.Command, executor, new(null, null));
        }
        else if (line.Statements != null)
        {
            var args = line.Statements.Select(x => x.Content);

            if (!args.Any())
            {
                return 0;
            }

            var cmdName = line.Statements[0].Content;
            var searchResult = PathSearcher.GetSystemExecutableName(Path.Combine(Environment.CurrentDirectory, cmdName));

            if (searchResult == null || !File.Exists(searchResult))
            {
                throw new CommandFormatException(Languages.UnknownCommand(cmdName));
            }

            var processInfo = new ProcessStartInfo(searchResult);
            args.ForEach(processInfo.ArgumentList.Add);
            processInfo.UseShellExecute = false;
            processInfo.WorkingDirectory = Environment.CurrentDirectory;

            var process = Process.Start(processInfo);

            if (process == null)
            {
                ConsoleOutput.PrintShellError(Languages.Get("shell_unable_start_process"));
                return -255;
            }

            process.WaitForExit();

            return process.ExitCode;
        }
        else
        {
            throw new ArgumentException("Invalid batch line", nameof(line));
        }
    }

    /// <summary>
    /// Executes a parsed command.
    /// </summary>
    /// <param name="command">The command to execute.</param>
    /// <param name="executor">The command executor to act on behalf of.</param>
    /// <param name="context">The context.</param>
    /// <returns>The exit code of the command.</returns>
    public int Execute(ParsedCommand command, ICommandExecutor executor, ExecutionContext context)
    {
        var instance = (ICommand)Activator.CreateInstance(command.Command.Type)!;

        if (command.Command.Definition.VariableValues &&
            command.Command.Definition.VariableValueBuffer != null)
        {
            command.Command.Definition.VariableValueBuffer.SetValue(instance, command.ArgumentParseResult.VariableValues.AsReadOnly());
        }

        foreach (var option in command.Command.Definition.Options.Select(x => x.Value))
        {
            if (!command.ArgumentParseResult.Options.TryGetValue(option, out var obj))
            {
                if (!option.HasValue)
                {
                    option.Property.SetValue(instance, false);
                }

                continue;
            }

            if (!option.HasValue)
            {
                option.Property.SetValue(instance, true);
            }

            option.Property.SetValue(instance, obj);
        }

        foreach (var value in command.Command.Definition.Values)
        {
            if (!command.ArgumentParseResult.Values.TryGetValue(value, out var obj))
            {
                continue;
            }

            value.Property.SetValue(instance, obj);
        }

        return instance.Execute(_shell, executor, context);
    }
}
