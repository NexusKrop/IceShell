// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Core.Commands;

using IceShell.Core.CLI.Languages;
using IceShell.Core.Exceptions;
using NexusKrop.IceShell.Core;
using NexusKrop.IceShell.Core.Commands.Complex;
using System;
using System.Collections.Generic;
using System.Linq;

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
    /// Parses a command string to a single parsed command.
    /// </summary>
    /// <param name="commandName">The name of the command to parse.</param>
    /// <param name="parser">The current command parser. Must be located after the name of the command.</param>
    /// <returns>The parsed command ready to execute.</returns>
    /// <exception cref="CommandFormatException">The command format is invalid.</exception>
    public static ParsedCommand Parse(string commandName, CommandParser parser)
    {
        var type = Shell.CommandManager.GetDefinition(commandName)
            ?? throw new CommandFormatException(Languages.UnknownCommand(commandName));
        var argument = new ComplexArgument(parser, type.Definition);

        var parsedArgs = argument.Parse();
        return new(parsedArgs, type);
    }

    /// <summary>
    /// Executes a parsed command.
    /// </summary>
    /// <param name="command">The command to execute.</param>
    /// <param name="executor">The command executor to act on behalf of.</param>
    /// <returns>The exit code of the command.</returns>
    public int Execute(ParsedCommand command, ICommandExecutor executor)
    {
        var instance = (NexusKrop.IceShell.Core.Commands.Complex.ICommand)Activator.CreateInstance(command.Command.Type)!;

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

        return instance.Execute(_shell, executor);
    }
}
