namespace NexusKrop.IceShell.Core.Commands.Bundled;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Command("ver", 0)]
public class VerCommand : ICommand
{
    private static readonly string PRODUCT_VERSION = FileVersionInfo.GetVersionInfo(Environment.ProcessPath ?? "NexusKrop.IceShell.exe").ProductVersion ?? "unknown";

    public void Execute(Shell shell, string[]? args)
    {
        Console.WriteLine(Messages.VerLine1, PRODUCT_VERSION);
        Console.WriteLine(Messages.VerLine2, Environment.OSVersion.VersionString);
        Console.WriteLine();
        Console.WriteLine(Messages.VerLine3);
    }
}
