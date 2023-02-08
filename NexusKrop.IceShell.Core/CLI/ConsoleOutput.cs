// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.CLI;
using System;

using static Crayon.Output;

public static class ConsoleOutput
{
    public static void PrintShellError(string message)
    {
        Console.WriteLine(Red(message));
    }

    public static void WriteLineColour(string message, ConsoleColor color)
    {
        var wasColor = Console.ForegroundColor;

        Console.ForegroundColor = color;
        Console.WriteLine(message);

        Console.ForegroundColor = wasColor;
    }
}
