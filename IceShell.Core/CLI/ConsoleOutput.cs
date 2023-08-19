// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.CLI;

using Spectre.Console;
using System;

public static class ConsoleOutput
{
    public static void PrintShellError(string message)
    {
        AnsiConsole.WriteLine($"[red]{Markup.Escape(message)}[/]");
    }

    public static void WriteLineColour(string message, ConsoleColor color)
    {
        var wasColor = Console.ForegroundColor;

        Console.ForegroundColor = color;
        Console.WriteLine(message);

        Console.ForegroundColor = wasColor;
    }
}
