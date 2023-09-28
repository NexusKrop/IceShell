// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Core.Commands;

using NexusKrop.IceShell.Core.Commands.Complex;
using static NexusKrop.IceShell.Core.Commands.CommandManager;

/// <summary>
/// Represents a parsed command.
/// </summary>
public record ParsedCommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ParsedCommand"/> structure.
    /// </summary>
    /// <param name="parseResult">The argument parse result.</param>
    /// <param name="command">The command to execute.</param>
    public ParsedCommand(ArgumentParseResult parseResult, ComplexCommandEntry command)
    {
        ArgumentParseResult = parseResult;
        Command = command;
    }

    /// <summary>
    /// Gets the parsed argument set.
    /// </summary>
    public ArgumentParseResult ArgumentParseResult { get; }

    /// <summary>
    /// Gets the command to execute.
    /// </summary>
    public ComplexCommandEntry Command { get; set; }
}
