// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Core.CLI;

public interface ITerminal
{
    TextWriter StandardOutput { get; }
    ConsoleKeyInfo ReadKey();
}
