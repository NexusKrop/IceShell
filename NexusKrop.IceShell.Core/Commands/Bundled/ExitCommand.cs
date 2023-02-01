namespace NexusKrop.IceShell.Core.Commands.Bundled;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Command("exit", 0)]
public class ExitCommand : ICommand
{
    public void Execute(Shell shell, string[]? args)
    {
        shell.Quit();
    }
}
