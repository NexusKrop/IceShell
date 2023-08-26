// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Core.CLI;

using System;
using System.IO;

public sealed class RealTerminal : ITerminal
{
    public TextWriter StandardOutput => Console.Out;

    public ConsoleKeyInfo ReadKey()
    {
        return Console.ReadKey();
    }
}
