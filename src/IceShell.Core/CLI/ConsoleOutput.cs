// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.CLI;

using Spectre.Console;
using System;

/// <summary>
/// Provides utilities to output console messages in various appearances. 
/// </summary>
public static class ConsoleOutput
{
    /// <summary>
    /// Prints the specified message in the style of an error message.
    /// </summary>
    /// <param name="message"></param>
    public static void PrintShellError(string message)
    {
        AnsiConsole.MarkupLine($"[red]{Markup.Escape(message)}[/]");
    }

    /// <summary>
    /// Writes the specified message, followed by the current line terminator,
    /// with the specified colour.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="color"></param>
    [Obsolete("Use Spectre.Console functions.")]
    public static void WriteLineColour(string message, ConsoleColor color)
    {
        var wasColor = Console.ForegroundColor;

        Console.ForegroundColor = color;
        Console.WriteLine(message);

        Console.ForegroundColor = wasColor;
    }
}
