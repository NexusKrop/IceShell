﻿// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Bundled;

using NexusKrop.IceShell.Core.Commands.Complex;

/// <summary>
/// Defines a command that exits the shell.
/// </summary>
[ComplexCommand("exit")]
public class ExitCommandEx : IComplexCommand
{
    public void Define(ComplexArgument argument)
    {
        // no arguments
    }

    public void Execute(ComplexArgumentParseResult argument)
    {
        Shell.Quit();
    }
}
