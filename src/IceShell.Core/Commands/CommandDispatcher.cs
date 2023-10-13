// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Core.Commands;

using IceShell.Core.Api;
using IceShell.Core.CLI.Languages;
using IceShell.Core.Commands.Argument;
using IceShell.Core.Exceptions;
using IceShell.Parsing;
using Microsoft.VisualBasic.FileIO;
using NexusKrop.IceCube.Util.Enumerables;
using NexusKrop.IceShell.Core.CLI;
using NexusKrop.IceShell.Core.Commands;
using NexusKrop.IceShell.Core.Commands.Complex;
using NexusKrop.IceShell.Core.FileSystem;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NewCommandParser = Parsing.CommandParser;

/// <summary>
/// Provides a service to shells that parses and executes commands on its behalf.
/// </summary>
public class CommandDispatcher : ICommandDispatcher
{
    private readonly CommandManager _manager;
    private readonly IShell _shell;

    private readonly ArgumentConvertService _convertService = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandDispatcher"/> class.
    /// </summary>
    /// <param name="shell">The shell to act on behalf of.</param>
    /// <param name="registerManagerDefaults">If <see langword="true"/>, the default commands for the manager will be registered.</param>
    public CommandDispatcher(IShell shell, bool registerManagerDefaults = true)
    {
        _shell = shell;
        _manager = new CommandManager(registerManagerDefaults);

        if (registerManagerDefaults)
        {
            _convertService.RegisterConverter(typeof(Enum), new EnumArgumentConverter());
            _convertService.RegisterConverter(typeof(string), new StringConverter());
        }
    }

    /// <inheritdoc />
    public ICommandManager CommandManager => _manager;

    /// <inheritdoc />
    public CommandSectionCompound ParseLine(string line)
    {
        var statements = new LineParser().ParseLine(line);

        var rawCompound = NewCommandParser.ParseCompound(statements, CommandManager.HasDefinition);

        var sysCompound = new List<CommandSection>();

        foreach (var state in rawCompound)
        {
            if (state.Type == SegmentType.File)
            {
                sysCompound.Add(new CommandSection(new List<SyntaxStatement>(state.FileStatements!), state.NextAction));
            }
            else if (state.Type == SegmentType.Command)
            {
                var definition = CommandManager.GetDefinition(state.Command!.Name)
                    ?? throw new InvalidOperationException("Check parser, invalid command");

                var argument = new CommandArgument(state.Command!, definition.Definition);
                var result = argument.Parse();

                var cmdInfo = new CommandUnit(result, definition);

                sysCompound.Add(new CommandSection(cmdInfo, state.Command!.Name, state.NextAction));
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
    public int Execute(CommandSectionCompound compound, ICommandExecutor executor)
    {
        var inAction = false;

        // This nextAction variable is the NextAction of the *previous* command.
        // For the NextAction for the current command, use context.NextAction.
        var nextAction = SyntaxNextAction.None;

        // The redirected standard output of the last command.
        TextReader? lastOutStream = null;

        foreach (var line in compound)
        {
            var context = new ExecutionContext(null, line.NextAction);

            if (nextAction == SyntaxNextAction.Redirect)
            {
                context.Retrieval = lastOutStream;
                lastOutStream = null;
            }

            // This default value (-2000) indicates that the entire routine below somehow did not run or was
            // interrupted.
            //
            // This is only useful in diagnostics if the process *did not start*! If the process did start,
            // the process it self can return -2000! Please do not assert this.
            int exitCode = -2000;
            TextReader? pipeStream = null;

            if (line.IsCommand)
            {
                exitCode = Execute(line, executor, out pipeStream, context);
            }
            else
            {
                var startInfo = Executive.CreateStartInfo(line);

                // RedirectTo: Redirect from last section to this section.
                var redirectTo = inAction && nextAction == SyntaxNextAction.Redirect && context.Retrieval != null;

                // RedirectFrom: Redirect from this section to next section.
                var redirectFrom = context.NextAction == SyntaxNextAction.Redirect;

                startInfo.RedirectStandardOutput = redirectFrom;
                startInfo.RedirectStandardInput = redirectTo;

                var process = Process.Start(startInfo)
                    ?? throw new InvalidOperationException("Failed to create process.");

                // The redirection of the standard input from last command.
                if (redirectTo)
                {
                    process.StandardInput.Write(context.Retrieval!.ReadToEnd());
                    context.Retrieval.Dispose();
                }

                Executive.WatchProcess(process);
                process.WaitForExit();

                // The redirection of the standard output from this command.
                if (redirectFrom)
                {
                    pipeStream = process.StandardOutput;
                }
            }

            if (exitCode != 0 && inAction)
            {
                // Command failure
                return exitCode;
            }

            lastOutStream = pipeStream;
            nextAction = line.NextAction;
            inAction = nextAction != SyntaxNextAction.None;
        }

        if (inAction)
        {
            throw new CommandFormatException(LangMessage.MsgActionNeverComplete());
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
    /// <param name="section">The line to execute.</param>
    /// <param name="executor">The executor to act on behalf of.</param>
    /// <param name="context">The execution context.</param>
    /// <param name="outStream">The pipe out stream.</param>
    /// <returns>The exit code of the command or process; if failed to start external command, returns <c>-255</c>.</returns>
    /// <exception cref="ArgumentException">The specified <see cref="CommandSection"/> is invalid.</exception>
    /// <exception cref="CommandFormatException">The specified command was not found.</exception>
    public int Execute(CommandSection section, ICommandExecutor executor, out TextReader? outStream, ExecutionContext? context = null)
    {
        if (string.IsNullOrWhiteSpace(section.Name))
        {
            outStream = null;
            return 0;
        }

        if (section.IsCommand && section.Command != null)
        {
            return Execute(section.Command, executor, out outStream, context ?? ExecutionContext.Default);
        }
        else if (section.Statements != null)
        {
            var args = section.Statements.Select(x => x.Content);

            if (!args.Any())
            {
                outStream = null;
                return 0;
            }

            var cmdName = section.Statements[0].Content;
            var searchResult = PathSearcher.GetSystemExecutableName(Path.Combine(Environment.CurrentDirectory, cmdName));

            if (searchResult == null || !File.Exists(searchResult))
            {
                throw new CommandFormatException(LangMessage.MsgUnknownCommand(cmdName));
            }

            var processInfo = new ProcessStartInfo(searchResult);
            args.ForEach(processInfo.ArgumentList.Add);
            processInfo.UseShellExecute = false;
            processInfo.WorkingDirectory = Environment.CurrentDirectory;

            var process = Process.Start(processInfo);

            if (process == null)
            {
                ConsoleOutput.PrintShellError(LangMessage.MsgUnableStartProcess());
                outStream = null;
                return -255;
            }

            process.WaitForExit();

            outStream = null;
            return process.ExitCode;
        }
        else
        {
            throw new ArgumentException("Invalid batch line", nameof(section));
        }
    }

    /// <summary>
    /// Executes a parsed command.
    /// </summary>
    /// <param name="command">The command to execute.</param>
    /// <param name="executor">The command executor to act on behalf of.</param>
    /// <param name="context">The context.</param>
    /// <param name="outStream">The pipe output stream.</param>
    /// <returns>The exit code of the command.</returns>
    public int Execute(CommandUnit command, ICommandExecutor executor, out TextReader? outStream, ExecutionContext context)
    {
        var instance = (ICommand)Activator.CreateInstance(command.Command.Type)!;

        if (command.Command.Definition.VariableValues &&
            command.Command.Definition.VariableValueBuffer != null)
        {
            command.Command.Definition.VariableValueBuffer.SetValue(instance, command.ArgumentParseResult.VariableValues.AsReadOnly());
        }

        // Process option arguments.
        foreach (var option in command.Command.Definition.Options.Select(x => x.Value))
        {
            if (!command.ArgumentParseResult.Options.TryGetValue(option, out var argStr))
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
                continue;
            }

            if (!_convertService.TryConvert(argStr ?? "", option.Property, instance))
            {
                throw ExceptionHelper.WithMessage(LangMessage.GetFormat("generic_arg_cannot_resolve", argStr ?? "<null>"));
            }
        }

        // Process value arguments.
        foreach (var definition in command.Command.Definition.Values)
        {
            if (!command.ArgumentParseResult.Values.TryGetValue(definition, out var argStr)
                || argStr == null)
            {
                continue;
            }

            if (!_convertService.TryConvert(argStr, definition.Property, instance))
            {
                throw ExceptionHelper.WithMessage(LangMessage.GetFormat("generic_arg_cannot_resolve", argStr));
            }

            definition.Property.SetValue(instance, argStr);
        }

        var retVal = instance.Execute(_shell, executor, context, out var pipeStream);
        outStream = pipeStream;
        return retVal;
    }
}
