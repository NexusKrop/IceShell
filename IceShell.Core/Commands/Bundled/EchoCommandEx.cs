// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Bundled;

using global::IceShell.Core;
using global::IceShell.Core.Commands;
using global::IceShell.Core.Commands.Attributes;
using NexusKrop.IceShell.Core.Commands.Complex;
using System;

/// <summary>
/// Defines a command that echoes or displays the specified text.
/// </summary>
[ComplexCommand("echo", "Displays messages.")]
[GreedyString]
public class EchoCommandEx : ICommand
{
    [Value("message", position: 0)]
    public string? Message { get; set; }

    public int Execute(IShell shell, ICommandExecutor executor)
    {
        Console.WriteLine(Message);
        return 0;
    }
}
