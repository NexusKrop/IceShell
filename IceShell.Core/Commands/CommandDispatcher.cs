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
    private readonly Shell _shell;

    public CommandDispatcher(Shell shell)
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

    public int Execute(ParsedCommand command)
    {
        var instance = (IComplexCommand)Activator.CreateInstance(command.Command.Type)!;

        foreach (var option in command.Command.Definition.Options)
        {
            option.Value.Property.SetValue(instance, command.ArgumentParseResult.Options[option.Value]);
        }

        foreach (var value in command.Command.Definition.Values)
        {
            value.Property.SetValue(instance, command.ArgumentParseResult.Values[value]);
        }

        return instance.Execute(command.ArgumentParseResult, _shell);
    }
}
