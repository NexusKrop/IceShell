namespace IceShell.Core.Commands;

using NexusKrop.IceShell.Core.Commands.Complex;
using System;
using static NexusKrop.IceShell.Core.Commands.CommandManager;

public class ParsedCommand
{
    public ParsedCommand(ComplexArgumentParseResult parseResult, ComplexCommandEntry command)
    {
        ArgumentParseResult = parseResult;
        Command = command;
    }

    public ComplexArgumentParseResult ArgumentParseResult { get; }
    public ComplexCommandEntry Command { get; set; }
}
