// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Bundled;

using global::IceShell.Core;
using global::IceShell.Core.Commands;
using NexusKrop.IceShell.Core.Commands.Complex;

/// <summary>
/// Defines a command that exits the shell.
/// </summary>
[ComplexCommand("exit", "Quits the IceShell program.")]
public class ExitCommandEx : ICommand
{
    public int Execute(IShell shell, ICommandExecutor executor)
    {
        shell.Quit();
        return 0;
    }
}
