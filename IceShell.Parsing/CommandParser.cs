namespace IceShell.Parsing;
using System;
using System.Collections.Generic;

/// <summary>
/// Provides utilities for parsing commands.
/// </summary>
public static class CommandParser
{
    /// <summary>
    /// The statement that specifies that all statements after it are values.
    /// </summary>
    public const string EndOfOptionsStatement = "--";

    /// <summary>
    /// Parses a single command.
    /// </summary>
    /// <param name="statements">The list of statements that constitutes of a command.</param>
    /// <returns>The parsed command.</returns>
    /// <exception cref="FormatException">The command syntax is invalid.</exception>
    public static SyntaxCommand ParseSingleCommand(SyntaxStatement[] statements)
    {
        var options = new HashSet<SyntaxOption>();
        var values = new HashSet<SyntaxStatement>();
        var name = "<pending>";
        var ignoreNextValue = false;
        var endOfOptions = false;

        for (int i = 0; i < statements.Length; i++)
        {
            var statement = statements[i];
            var content = statement.Content;

            if (i == 0)
            {
                name = content;
                continue;
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                continue;
            }

            if (!endOfOptions && content == EndOfOptionsStatement)
            {
                endOfOptions = true;
            }

            if (statement.WasQuoted
                || content.Length == 1
                || endOfOptions
                || !content.StartsWith('/'))
            {
                // Is value!
                if (!ignoreNextValue)
                {
                    values.Add(statement);
                }

                ignoreNextValue = false;
                continue;
            }

            SyntaxStatement? next = null;

            if (statements.Length > i + 1)
            {
                next = statements[i + 1];
                ignoreNextValue = true;
            }

            options.Add(ParseOption(statement, next));
        }

        return new(name, options, values);
    }

    /// <summary>
    /// Parses a single option.
    /// </summary>
    /// <param name="statement">The first statement.</param>
    /// <param name="next">The statement directly after the first statement.</param>
    /// <returns>The parsed option.</returns>
    /// <exception cref="FormatException">The option syntax is invalid.</exception>
    public static SyntaxOption ParseOption(SyntaxStatement statement, SyntaxStatement? next)
    {
        var content = statement.Content;
        string? value;

        var delimiter = content[1];

        if (content.Length == 2)
        {
            value = null;
        }
        else if (content.Length == 3 && content.EndsWith(':'))
        {
            if (next == null)
            {
                throw new FormatException(string.Format("Next statement is required for option '{0}'", delimiter));
            }

            if (!next.WasQuoted)
            {
                throw new FormatException(string.Format("Invalid option '{0}'. Is parsing failure?", delimiter));
            }

            value = next.Content;
        }
        else
        {
            value = content[3..];
        }

        return new(delimiter, value);
    }
}
