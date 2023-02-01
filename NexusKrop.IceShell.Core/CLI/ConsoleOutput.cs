namespace NexusKrop.IceShell.Core.CLI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class ConsoleOutput
{
    public static void PrintShellError(string message)
    {
        WriteLineColour(message, ConsoleColor.Red);
        Console.WriteLine();
    }

    public static void WriteLineColour(string message, ConsoleColor color)
    {
        var wasColor = Console.ForegroundColor;

        Console.ForegroundColor = color;
        Console.WriteLine(message);

        Console.ForegroundColor = wasColor;
    }
}
