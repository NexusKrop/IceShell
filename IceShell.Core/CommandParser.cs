// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core;

using global::IceShell.Core.CLI.Languages;
using NexusKrop.IceShell.Core.Exceptions;
using System.Text;

public class CommandParser
{
    public const char WHITESPACE = ' ';
    public const char DOUBLE_QUOTE = '"';
    public const char ESCAPE = '\\';

    public CommandParser()
    {
    }

    internal CommandParser(string line, int position)
    {
        Line = line;
        Position = position;
    }

    public string Line { get; private set; } = string.Empty;

    public int Length => Line.Length;
    public int Position { get; set; }

    public bool CanRead(int offset = 1)
    {
        return Position + offset <= Length;
    }

    internal void Clear()
    {
        Line = string.Empty;
        Position = 0;
    }

    public void SetLine(string line)
    {
        Clear();
        Line = line;
    }

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

    public void Skip()
    {
        Position++;
    }

    public char Read()
    {
        return Line[Position++];
    }

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
