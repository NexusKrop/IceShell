namespace IceShell.Core.Commands;

using IceShell.Parsing;
using System.Collections.Generic;

/// <summary>
/// Represents a line of command.
/// </summary>
public record BatchLine
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BatchLine"/> record, with a parsed internal command.
    /// </summary>
    /// <param name="parsedCommand">The parsed internal command.</param>
    /// <param name="commandName">The name of the command.</param>
    public BatchLine(ParsedCommand parsedCommand, string commandName)
    {
        IsCommand = true;
        Command = parsedCommand;
        Name = commandName;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BatchLine"/> record, with a list of statements to pass to
    /// an external program.
    /// </summary>
    /// <param name="statements">The list of statements containing the name of the command.</param>
    public BatchLine(IList<SyntaxStatement> statements)
    {
        IsCommand = false;
        Statements = statements;
        Name = statements[0].Content;
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
    public ParsedCommand? Command { get; }
}
