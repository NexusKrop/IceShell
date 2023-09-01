// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.CLI;

using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Provides methods for providing interactive questions.
/// </summary>
public static class ConsoleAsk
{
    /// <summary>
    /// Asks the user for Yes or No, with the specified message. If user did not provide Yes or No, the question
    /// is repeated.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <returns><see langword="true"/> if Yes, and <see langword="false"/> if No.</returns>
    public static bool YesNo(string message)
    {
        var incorrectRun = false;

        while (true)
        {
            if (incorrectRun)
            {
                AnsiConsole.MarkupLine("[red]Invalid answer! Y for Yes, and N for No![/]");
            }

            Console.Write($"{message} (Y/N) ");

            var key = Console.ReadKey();
            Console.WriteLine();

            if (key.Key == ConsoleKey.Y)
            {
                return true;
            }
            else if (key.Key == ConsoleKey.N)
            {
                return false;
            }

            incorrectRun = true;
        }
    }
}
