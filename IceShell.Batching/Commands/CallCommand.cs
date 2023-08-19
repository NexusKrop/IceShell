namespace IceShell.Batching.Commands;

using IceShell.Core;
using IceShell.Core.Commands;
using IceShell.Core.Commands.Attributes;
using Microsoft.Extensions.FileSystemGlobbing;
using NexusKrop.IceShell.Core.Commands;
using NexusKrop.IceShell.Core.Commands.Complex;

[ComplexCommand("call", description: "Calls a batch file.")]
public class CallCommand : ICommand
{
    [Value("file", true, 0)]
    public string? FileName { get; set; }

    public int Execute(IShell shell, ICommandExecutor executor)
    {
        var full = Path.GetFullPath(FileName!);

        CommandChecks.FileExists(full);

        BatchFile.Parse(File.ReadAllLines(full)).RunBatch(shell);
        return 0;
    }
}
