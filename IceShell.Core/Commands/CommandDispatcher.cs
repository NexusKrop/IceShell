// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Core.Commands;

using IceShell.Core.CLI.Languages;
using Microsoft.VisualBasic.FileIO;
using NexusKrop.IceShell.Core;
using NexusKrop.IceShell.Core.Commands;
using NexusKrop.IceShell.Core.Commands.Complex;
using NexusKrop.IceShell.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

public class CommandDispatcher
{
    private readonly IShell _shell;

    public CommandDispatcher(IShell shell)
    {
        _shell = shell;
    }

    public static ParsedCommand Parse(string commandName, CommandParser parser)
    {
        var type = Shell.CommandManager.GetDefinition(commandName)
            ?? throw new CommandFormatException(Languages.UnknownCommand(commandName));
        var argument = new ComplexArgument(parser, type.Definition);

        var parsedArgs = argument.Parse();
        return new(parsedArgs, type);
    }

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
