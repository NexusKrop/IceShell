// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Bundled;

using global::IceShell.Core;
using global::IceShell.Core.Commands;
using global::IceShell.Core.Commands.Attributes;
using NexusKrop.IceShell.Core.Commands.Complex;

/// <summary>
/// Defines a command that does nothing and always succeeds. This command is to be used as a comment command.
/// </summary>
[ComplexCommand("rm")]
[GreedyString]
public class RemarksCommand : ICommand
{
    /// <summary>
    /// Gets or sets the content of the comment.
    /// </summary>
    [Value("comment", position: 0)]
    public string? Comment { get; set; }

    /// <inheritdoc />
    public int Execute(IShell shell, ICommandExecutor executor, ExecutionContext context, out TextReader? pipeStream)
    {
        pipeStream = null;
        return 0;
    }
}
