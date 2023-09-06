// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

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
        var values = new List<SyntaxStatement>();
        var name = "<pending>";
        var ignoreNextValue = false;
        var endOfOptions = false;

        if (!statements.Any())
        {
            name = string.Empty;
            return new(name, options, values);
        }

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

            if (!endOfOptions
                && content == EndOfOptionsStatement
                && !statement.WasQuoted)
            {
                endOfOptions = true;
                continue;
            }

            if (endOfOptions || !IsOption(statement))
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
            }

            var option = ParseOption(statement, next, out var extended);

            if (extended)
            {
                ignoreNextValue = true;
            }

            options.Add(option);
        }

        return new(name, options, values);
    }

    /// <summary>
    /// Parses a single option.
    /// </summary>
    /// <param name="statement">The first statement.</param>
    /// <param name="next">The statement directly after the first statement.</param>
    /// <param name="extended">Whether the next value is used to parse the option. The output value of this parameter should be used to tell the parser to skip the next value, if needed.</param>
    /// <returns>The parsed option.</returns>
    /// <exception cref="FormatException">The option syntax is invalid.</exception>
    public static SyntaxOption ParseOption(SyntaxStatement statement, SyntaxStatement? next, out bool extended)
    {
        var content = statement.Content;
        string? value;

        var delimiter = content[1];

        if (content.Length == 2)
        {
            extended = false;
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

            extended = true;
            value = next.Content;
        }
        else
        {
            extended = false;
            value = content[3..];
        }

        return new(delimiter, value);
    }

    internal static bool IsOption(SyntaxStatement statement)
    {
        var content = statement.Content;

        if (statement.WasQuoted)
        {
            return false;
        }

        if (content.Length < 2)
        {
            return false;
        }

        if (!content.StartsWith('/'))
        {
            return false;
        }

        if (content.Length > 2 && content[2] != ':')
        {
            return false;
        }

        return true;
    }
}
