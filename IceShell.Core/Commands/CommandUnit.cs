// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Core.Commands;

using NexusKrop.IceShell.Core.Commands.Complex;

/// <summary>
/// Represents a parsed command.
/// </summary>
public record CommandUnit
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CommandUnit"/> structure.
    /// </summary>
    /// <param name="parseResult">The argument parse result.</param>
    /// <param name="command">The command to execute.</param>
    public CommandUnit(ArgumentParseResult parseResult, CommandEntry command)
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
    public CommandEntry Command { get; set; }
}
