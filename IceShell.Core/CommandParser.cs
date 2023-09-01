// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core;

using global::IceShell.Core.CLI.Languages;
using global::IceShell.Core.Exceptions;
using System.Text;

/// <summary>
/// Provides command parsing infrastructure to interactive shells.
/// </summary>
public class CommandParser
{
    /// <summary>
    /// The whitespace character.
    /// </summary>
    public const char WHITESPACE = ' ';

    /// <summary>
    /// The double-quote character.
    /// </summary>
    public const char DOUBLE_QUOTE = '"';

    /// <summary>
    /// The escape (backward-slash) character.
    /// </summary>
    public const char ESCAPE = '\\';

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandParser"/> class.
    /// </summary>
    public CommandParser()
    {
    }

    internal CommandParser(string line, int position)
    {
        Line = line;
        Position = position;
    }

    /// <summary>
    /// Gets the line that is currently being parsed.
    /// </summary>
    public string Line { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the length of the currently parsing line of this instance.
    /// </summary>
    public int Length => Line.Length;

    /// <summary>
    /// Gets or sets the position of the parsing "cursor" of this instance.
    /// </summary>
    public int Position { get; set; }

    /// <summary>
    /// Determines whether this instance should be able to read (or peek) at the specified offset after the
    /// current position, without triggering <see cref="IndexOutOfRangeException"/>.
    /// </summary>
    /// <param name="offset">The offset.</param>
    /// <returns>If <see langword="true"/>, this instance should be able to read at the specified offset after the
    /// current position; otherwise, <see langword="false"/>.</returns>
    public bool CanRead(int offset = 1)
    {
        return Position + offset <= Length;
    }

    /// <summary>
    /// Resets the cursor.
    /// </summary>
    internal void Clear()
    {
        Line = string.Empty;
        Position = 0;
    }

    /// <summary>
    /// Reset this instance, and sets the currently parsing line to the specified line.
    /// </summary>
    /// <param name="line">The line to parse.</param>
    public void SetLine(string line)
    {
        Clear();
        Line = line;
    }

    /// <summary>
    /// Gets the character at the specified position without advancing the cursor.
    /// </summary>
    /// <param name="offset"></param>
    /// <returns>If the offset is valid, returns the character at the offset; otherwise, <see langword="null"/>.</returns>
    public char? Peek(int offset = 0)
    {
        if (!CanRead(offset))
        {
            return null;
        }

        try
        {
            return Line[Position + offset];
        }
        catch (IndexOutOfRangeException)
        {
            return null;
        }
    }

    /// <summary>
    /// Skips the current character.
    /// </summary>
    public void Skip()
    {
        Position++;
    }

    /// <summary>
    /// Returns the current character and then advances the cursor by 1.
    /// </summary>
    /// <returns>The current character.</returns>
    /// <remarks>
    /// <note type="warning">
    /// Please do check <see cref="CanRead(int)"/> because <see cref="IndexOutOfRangeException"/> may be thrown if the current position
    /// turns out to be unreadable.
    /// </note>
    /// </remarks>
    public char Read()
    {
        return Line[Position++];
    }

    /// <summary>
    /// Reads a string from the current position to the end of the line, and advance the cursor to the
    /// end of the line.
    /// </summary>
    /// <returns>A string from the current position to the end of the line.</returns>
    /// <exception cref="CommandFormatException">Unable to read a string.</exception>
    public string ReadToEnd()
    {
        if (!CanRead())
        {
            throw new CommandFormatException(Languages.Get("reader_excepts_string"));
        }

        var builder = new StringBuilder();

        while (CanRead())
        {
            builder.Append(Read());
        }

        return builder.ToString();
    }

    /// <summary>
    /// Parses arguments to feed to other programs.
    /// </summary>
    /// <param name="args">The output arguments.</param>
    public void ReadArgs(out string[]? args)
    {
        List<string> ar = new();

        while (CanRead())
        {
            var str = ReadString();

            if (string.IsNullOrWhiteSpace(str))
            {
                continue;
            }

            ar.Add(str);
        }

        args = ar.ToArray();
    }

    /// <summary>
    /// Reads either an unquoted string or a quoted string, depending on the current character.
    /// </summary>
    /// <returns>An unquoted string or a quoted string.</returns>
    public string? ReadString()
    {
        if (Peek() == DOUBLE_QUOTE)
        {
            return ReadQuotedString();
        }
        else
        {
            return ReadUnquotedString();
        }
    }

    /// <summary>
    /// Reads A quoted string. The first double quote is assumed started at the current position.
    /// </summary>
    /// <returns>The unquoted string; if nothing to read, returns <see langword="null"/>.</returns>
    /// <remarks>
    /// <para>
    /// For a quoted string to be valid, the first double quote must be exactly at the cursor, and the second double quote must
    /// be exist.
    /// </para>
    /// <para>
    /// This method supports escaping characters to ignore special characters that needs to be escaped; to escape
    /// a character, add a backslash before the character you want to escape.
    /// </para>
    /// </remarks>
    /// <exception cref="CommandFormatException">The quoted string is invalid.</exception>
    public string? ReadQuotedString()
    {
        if (!CanRead())
        {
            return null;
        }

        if (Peek() != DOUBLE_QUOTE)
        {
            throw new CommandFormatException(Languages.Get("reader_excepts_begin_of_quote"));
        }

        var builder = new StringBuilder();

        Skip();

        var escaping = false;

        while (true)
        {
            if (!CanRead())
            {
                throw new CommandFormatException(Languages.Get("reader_quoted_string_never_ends"));
            }

            var c = Read();

            if (c == ESCAPE)
            {
                escaping = true;
                continue;
            }

            if (escaping)
            {
                escaping = false;
                builder.Append(c);
                continue;
            }

            if (c == DOUBLE_QUOTE)
            {
                break;
            }

            builder.Append(c);
        }

        return builder.ToString();
    }

    /// <summary>
    /// Reads an unquoted string. The start of the unquoted string is assumed to be at the current position.
    /// </summary>
    /// <returns>The unquoted string. If there is nothing to read, returns <see langword="null"/>.</returns>
    public string? ReadUnquotedString()
    {
        if (!CanRead())
        {
            return null;
        }

        var builder = new StringBuilder();

        while (true)
        {
            if (!CanRead())
            {
                break;
            }

            var c = Read();

            if (c == WHITESPACE)
            {
                break;
            }

            builder.Append(c);
        }

        if (builder.Length == 0)
        {
            return null;
        }

        return builder.ToString();
    }
}
