// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Bundled;

using global::IceShell.Core;
using global::IceShell.Core.Commands;
using NexusKrop.IceShell.Core.Commands.Complex;
using System;

/// <summary>
/// Defines a command that clears the console screen.
/// </summary>
/// <seealso cref="Console.Clear()"/>
[ComplexCommand("cls", "Clears the screen.")]
public class ClsCommandEx : ICommand
{
    public int Execute(IShell shell, ICommandExecutor executor)
    {
        Console.Clear();
        return 0;
    }
}
