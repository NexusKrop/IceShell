// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Bundled;

using NexusKrop.IceShell.Core.Commands.Complex;
using System;

/// <summary>
/// Defines a command that clears the console screen.
/// </summary>
/// <seealso cref="Console.Clear()"/>
[ComplexCommand("cls", "Clears the screen.")]
public class ClsCommandEx : IComplexCommand
{
    public int Execute(ComplexArgumentParseResult argument, Shell shell)
    {
        Console.Clear();
        return 0;
    }
}
