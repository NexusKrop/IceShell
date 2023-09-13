// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Bundled;

using global::IceShell.Core;
using global::IceShell.Core.Commands;
using global::IceShell.Core.Commands.Attributes;
using NexusKrop.IceShell.Core.Commands.Complex;
using System;

/// <summary>
/// Displays messages.
/// </summary>
[ComplexCommand("echo", "Displays messages.")]
[GreedyString]
public class EchoCommandEx : ICommand
{
    /// <summary>
    /// Gets or sets the message to display.
    /// </summary>
    /// <value>
    /// The message to display. If <see langword="null"/>, displays a blank new line.
    /// </value>
    [Value("message", position: 0)]
    public string? Message { get; set; }

    /// <inheritdoc/>
    public int Execute(IShell shell, ICommandExecutor executor, ExecutionContext context)
    {
        Console.WriteLine(Message ?? Environment.NewLine);
        return 0;
    }
}
