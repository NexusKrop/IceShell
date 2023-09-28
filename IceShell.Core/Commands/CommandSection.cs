// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Core.Commands;

using IceShell.Parsing;
using System.Collections.Generic;

/// <summary>
/// Represents a line of command.
/// </summary>
public record CommandSection
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CommandSection"/> record.
    /// </summary>
    public CommandSection()
    {
        IsCommand = false;
        Name = string.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandSection"/> record, with a parsed internal command.
    /// </summary>
    /// <param name="parsedCommand">The parsed internal command.</param>
    /// <param name="commandName">The name of the command.</param>
    /// <param name="nextAction">The action to perform to the next command.</param>
    public CommandSection(CommandUnit parsedCommand, string commandName, SyntaxNextAction nextAction = SyntaxNextAction.None)
    {
        IsCommand = true;
        Command = parsedCommand;
        Name = commandName;
        NextAction = nextAction;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandSection"/> record, with a list of statements to pass to
    /// an external program.
    /// </summary>
    /// <param name="statements">The list of statements containing the name of the command.</param>
    /// <param name="nextAction">The action to perform to the next command.</param>
    public CommandSection(IList<SyntaxStatement> statements, SyntaxNextAction nextAction = SyntaxNextAction.None)
    {
        IsCommand = false;
        Statements = statements;
        Name = statements[0].Content;
        NextAction = nextAction;
    }

    /// <summary>
    /// Gets whether this batch line invokes an internal command.
    /// </summary>
    public bool IsCommand { get; }

    /// <summary>
    /// Gets the name of the command this batch line invokes.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets a list of statements.
    /// </summary>
    public IList<SyntaxStatement>? Statements { get; }

    /// <summary>
    /// Gets the parsed command.
    /// </summary>
    public CommandUnit? Command { get; }

    /// <summary>
    /// Gets the action to perform to the next command in the compound.
    /// </summary>
    public SyntaxNextAction NextAction { get; }
}
