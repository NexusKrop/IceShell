// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Bundled;

using global::IceShell.Core;
using global::IceShell.Core.Commands;
using NexusKrop.IceShell.Core.Commands.Complex;

/// <summary>
/// Quits the IceShell program.
/// </summary>
[ComplexCommand("exit", "Quits the IceShell program.")]
public class ExitCommandEx : ICommand
{
    /// <inheritdoc/>
    public int Execute(IShell shell, ICommandExecutor executor, ExecutionContext context, out TextReader? pipeStream)
    {
        pipeStream = null;
        shell.Quit();
        return 0;
    }
}
