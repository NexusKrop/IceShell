namespace NexusKrop.IceShell.Core.Commands.Bundled;

using NexusKrop.IceCube.Exceptions;
using NexusKrop.IceShell.Core.Commands.Complex;
using NexusKrop.IceShell.Core.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[ComplexCommand("cd")]
public class CdCommandEx : IComplexCommand
{
    public void Define(ComplexArgument argument)
    {
        argument.AddValue(new("destination", true));
    }

    public void Execute(ComplexArgumentParseResult argument)
    {
        var target = PathSearcher.ShellToSystem(argument.Values[0]!);

        Checks.DirectoryExists(target);

        Shell.ChangeDirectory(target);
    }
}
