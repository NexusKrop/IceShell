// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Core.Commands;

using IceShell.Core.CLI.Languages;
using IceShell.Core.Exceptions;
using IceShell.Parsing;
using NexusKrop.IceCube;
using NexusKrop.IceCube.Util.Enumerables;
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
    public static BatchLineCompound ParseLine(string line)
    {
        var statements = new LineParser().ParseLine(line);

        var rawCompound = NewCommandParser.ParseCompound(statements, Shell.CommandManager.HasDefinition);

        var sysCompound = new List<BatchLine>();

        foreach (var state in rawCompound)
        {
            if (state.Type == SegmentType.File)
            {
                sysCompound.Add(new BatchLine(new List<SyntaxStatement>(state.FileStatements!), state.NextAction));
            }
            else if (state.Type == SegmentType.Command)
            {
                var definition = Shell.CommandManager.GetDefinition(state.Command!.Name)
                    ?? throw new InvalidOperationException("Check parser, invalid command");

                var argument = new CommandArgument(state.Command!, definition.Definition);
                var result = argument.Parse();

                var cmdInfo = new ParsedCommand(result, definition);

                sysCompound.Add(new BatchLine(cmdInfo, state.Command!.Name, state.NextAction));
            }
        }

        return new(sysCompound);
    }

    /// <summary>
    /// Executes a batch line compound.
    /// </summary>
    /// <param name="compound">The compound to execute..</param>
    /// <param name="executor">Th executor to act on behalf of.</param>
    /// <returns>The return code of the commands. Returns zero if success.</returns>
    /// <exception cref="CommandFormatException">Action never ends, or other kinds of command failures.</exception>
    public int Execute(BatchLineCompound compound, ICommandExecutor executor)
    {
        var inAction = false;
        var nextAction = SyntaxNextAction.None;
        TextReader? lastOutStream = null;

        foreach (var line in compound)
        {
            var context = new ExecutionContext(null, line.NextAction);

            if (nextAction == SyntaxNextAction.Redirect)
            {
                context.Retrieval = lastOutStream;
                lastOutStream = null;
            }

            var exitCode = Execute(line, executor, out var pipeStream, context);

            if (exitCode != 0 && inAction)
            {
                // Command failure
                return exitCode;
            }

            if (line.NextAction == SyntaxNextAction.Redirect)
            {
                lastOutStream = pipeStream;
            }

            nextAction = line.NextAction;
            inAction = nextAction != SyntaxNextAction.None;
        }

        if (inAction)
        {
            throw new CommandFormatException(Languages.ActionNeverComplete());
        }

        if (lastOutStream != null)
        {
            Console.WriteLine(lastOutStream.ReadToEnd());
        }

        return 0;
    }

    /// <summary>
    /// Executes the specified batch line.
    /// </summary>
    /// <param name="line">The line to execute.</param>
    /// <param name="executor">The executor to act on behalf of.</param>
    /// <param name="context">The execution context.</param>
    /// <param name="outStream">The pipe out stream.</param>
    /// <returns>The exit code of the command or process; if failed to start external command, returns <c>-255</c>.</returns>
    /// <exception cref="ArgumentException">The specified <see cref="BatchLine"/> is invalid.</exception>
    /// <exception cref="CommandFormatException">The specified command was not found.</exception>
    public int Execute(BatchLine line, ICommandExecutor executor, out TextReader? outStream, ExecutionContext? context = null)
    {
        if (string.IsNullOrWhiteSpace(line.Name))
        {
            outStream = null;
            return 0;
        }

        if (line.IsCommand && line.Command != null)
        {
            return Execute(line.Command, executor, context ?? ExecutionContext.Empty, out outStream);
        }
        else if (line.Statements != null)
        {
            var args = line.Statements.Select(x => x.Content);

            if (!args.Any())
            {
                outStream = null;
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
                outStream = null;
                return -255;
            }

            process.WaitForExit();

            outStream = null;
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
    /// <param name="pipeOut">The pipe output stream.</param>
    /// <returns>The exit code of the command.</returns>
    public int Execute(ParsedCommand command, ICommandExecutor executor, ExecutionContext context, out TextReader? pipeOut)
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

        var retVal = instance.Execute(_shell, executor, context, out var pipeStream);
        pipeOut = pipeStream;
        return retVal;
    }
}
