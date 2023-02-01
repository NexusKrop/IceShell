﻿namespace NexusKrop.IceShell.Core;

using NexusKrop.IceShell.Core.Exceptions;
using System.Collections.Specialized;
using System.Text;

public class CommandParser
{
    public const char WHITESPACE = ' ';
    public const char DOUBLE_QUOTE = '"';

    public CommandParser()
    {
    }

    public CommandParser(string line, int position)
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

    public void Clear()
    {
        Line = string.Empty;
        Position = 0;
    }

    public void SetLine(string line)
    {
        Clear();
        Line = line;
    }

    public char Peek(int offset = 0)
    {
        return Line[Position + offset];
    }

    public void Skip()
    {
        Position++;
    }

    public char Read()
    {
        return Line[Position++];
    }

    public void ReadCommand(out string? command, out string[]? args)
    {
        command = ReadString();

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
            throw new CommandFormatException(ER.ExceptedBeginOfQuote);
        }

        var builder = new StringBuilder();

        Skip();

        while (true)
        {
            if (!CanRead())
            {
                throw new CommandFormatException(ER.ExceptedEndOfQuote);
            }

            var c = Read();

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
