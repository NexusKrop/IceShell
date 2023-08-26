// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.CLI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class ConsoleAsk
{
    public static bool YesNo(string message)
    {
        while (true)
        {
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
        }
    }
}
