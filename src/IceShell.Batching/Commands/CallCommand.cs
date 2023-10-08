// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Batching.Commands;

using IceShell.Core;
using IceShell.Core.Commands;
using IceShell.Core.Commands.Attributes;
using NexusKrop.IceShell.Core.Commands.Complex;

[ComplexCommand("call", description: "Calls a batch file.")]
public class CallCommand : ICommand
{
    [Value("file", true, 0)]
    public string? FileName { get; set; }

    public int Execute(IShell shell, ICommandExecutor executor, ExecutionContext context, out TextReader? pipeStream)
    {
        pipeStream = null;

        var full = Path.GetFullPath(FileName!);

        CommandChecks.FileExists(full);

        BatchFile.Parse(File.ReadAllLines(full), shell.Dispatcher).RunBatch(shell);
        return 0;
    }
}
