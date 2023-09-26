// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Batching.Commands;

using IceShell.Core;
using IceShell.Core.CLI.Languages;
using IceShell.Core.Commands;
using IceShell.Core.Commands.Attributes;
using IceShell.Core.Exceptions;
using NexusKrop.IceShell.Core.Commands.Complex;

[ComplexCommand("goto", Description = "In a batch file, jump to the specified label.")]
public class GotoCommand : ICommand
{
    [Value("label", true, 0)]
    public string? Label { get; set; }

    public int Execute(IShell shell, ICommandExecutor executor, ExecutionContext context, out TextReader? pipeStream)
    {
        pipeStream = null;

        if (!executor.SupportsJump)
        {
            throw new CommandFormatException(Languages.Get("batch_goto_not_supported"));
        }

        executor.Jump(Label!);
        return 0;
    }
}
