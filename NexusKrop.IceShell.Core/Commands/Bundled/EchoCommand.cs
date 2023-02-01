namespace NexusKrop.IceShell.Core.Commands.Bundled;

using NexusKrop.IceShell.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Command("echo", 1)]
public class EchoCommand : ICommand
{
    public void Execute(Shell shell, string[]? args)
    {
        ArgumentNullException.ThrowIfNull(args);

        Console.WriteLine(args[0]);
    }
}
