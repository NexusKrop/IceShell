namespace NexusKrop.IceShell.Core.Commands.Bundled;

using NexusKrop.IceShell.Core.Commands.Complex;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[ComplexCommand("ver")]
public class VerCommand : IComplexCommand
{
    private static readonly string PRODUCT_VERSION = FileVersionInfo.GetVersionInfo(Environment.ProcessPath ?? "NexusKrop.IceShell.exe").ProductVersion ?? "unknown";

    public void Define(ComplexArgument argument)
    {
        // No arguments
    }

    public void Execute(ComplexArgumentParseResult argument)
    {
        Console.WriteLine(Messages.VerLine1, PRODUCT_VERSION);
        Console.WriteLine(Messages.VerLine2, Environment.OSVersion.VersionString);
        Console.WriteLine();
        Console.WriteLine(Messages.VerLine3);
    }
}
