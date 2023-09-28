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
    [Value("message", position: 0, required: false)]
    public string? Message { get; set; }

    /// <inheritdoc/>
    public int Execute(IShell shell, ICommandExecutor executor, ExecutionContext context, out TextReader? pipeStream)
    {
        pipeStream = null;

        if (context.Retrieval == null)
        {
            Console.WriteLine(Message ?? Environment.NewLine);
        }
        else
        {
            Console.WriteLine(context.Retrieval.ReadToEnd());
        }

        return 0;
    }
}
