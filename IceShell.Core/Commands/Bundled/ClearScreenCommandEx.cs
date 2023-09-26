// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Bundled;

using global::IceShell.Core;
using global::IceShell.Core.Commands;
using NexusKrop.IceShell.Core.Commands.Complex;
using System;

/// <summary>
/// Clears the screen.
/// </summary>
/// <seealso cref="Console.Clear()"/>
[ComplexCommand("cls", "Clears the screen.")]
public class ClearScreenCommandEx : ICommand
{
    /// <inheritdoc />
    public int Execute(IShell shell, ICommandExecutor executor, ExecutionContext context, out TextReader? pipeStream)
    {
        pipeStream = null;
        Console.Clear();
        return 0;
    }
}
