// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Bundled;

using NexusKrop.IceShell.Core.Commands.Complex;

/// <summary>
/// Defines a command that exits the shell.
/// </summary>
[ComplexCommand("exit", "Quits the IceShell program.")]
public class ExitCommandEx : IComplexCommand
{
    public void Define(ComplexArgument argument)
    {
        // no arguments
    }

    public int Execute(ComplexArgumentParseResult argument)
    {
        Shell.Quit();
        return 0;
    }
}
