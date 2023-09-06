// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Parsing;

/// <summary>
/// Represents a command syntax statement, the basis for all command arguments (options, values, etc.).
/// </summary>
public record SyntaxStatement
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SyntaxStatement"/> class.
    /// </summary>
    /// <param name="content">The string content.</param>
    /// <param name="wasQuoted">Whether the statement was specified via a quoted string.</param>
    public SyntaxStatement(string content, bool wasQuoted)
    {
        Content = content;
        WasQuoted = wasQuoted;
    }

    /// <summary>
    /// Gets or sets the content of this statement.
    /// </summary>
    public string Content { get; }

    /// <summary>
    /// Gets or sets whether this statement was quoted.
    /// </summary>
    public bool WasQuoted { get; }
}
