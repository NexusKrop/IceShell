// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Batching.Commands;

using IceShell.Core;
using IceShell.Core.Api;
using IceShell.Core.Commands;
using IceShell.Core.Commands.Attributes;
using NexusKrop.IceShell.Core.Commands.Complex;

[ComplexCommand("call", description: "Calls a batch file.")]
public class CallCommand : IShellCommand
{
    [Value("file", true, 0)]
    public string? FileName { get; set; }

    public CommandResult Execute(IShell shell, ICommandExecutor executor, ExecutionContext context)
    {
        var full = Path.GetFullPath(FileName!);

        CommandChecks.FileExists(full);

        return BatchFile.Parse(File.ReadAllLines(full), shell.Dispatcher).RunBatch(shell);
    }
}
