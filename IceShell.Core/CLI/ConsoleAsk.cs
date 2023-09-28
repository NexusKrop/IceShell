// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.CLI;

using global::IceShell.Core.Api;
using Spectre.Console;
using System;

/// <summary>
/// Provides methods for providing interactive questions.
/// </summary>
public static class ConsoleAsk
{
    /// <summary>
    /// Asks the user for Yes or No, with the specified message. If user did not provide Yes or No, the question
    /// is repeated indefinitely until the user provided a correct answer.
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

            var key = Console.ReadLine();

            Console.WriteLine();

            switch (SystemService.MatchYesNo(key ?? string.Empty))
            {
                case YesNoAnswer.Invalid:
                    incorrectRun = true;
                    continue;

                case YesNoAnswer.Yes:
                    return true;

                case YesNoAnswer.No:
                    return false;
            }
        }
    }
}
