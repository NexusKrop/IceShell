namespace NexusKrop.IceShell.Core.Commands.Bundled;

using NexusKrop.IceCube.Exceptions;
using NexusKrop.IceShell.Core.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Command("cd", 1)]
public class CdCommand : ICommand
{
    public void Execute(Shell shell, string[]? args)
    {
        ArgumentNullException.ThrowIfNull(args);

        var target = PathSearcher.ShellToSystem(args[0]);

        Checks.DirectoryExists(target);

        Directory.SetCurrentDirectory(target);
    }
}
