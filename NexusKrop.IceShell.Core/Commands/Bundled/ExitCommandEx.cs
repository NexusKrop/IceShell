namespace NexusKrop.IceShell.Core.Commands.Bundled;

using NexusKrop.IceShell.Core.Commands.Complex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[ComplexCommand("exit")]
public class ExitCommandEx : IComplexCommand
{
    public void Define(ComplexArgument argument)
    {
        // no arguments
    }

    public void Execute(ComplexArgumentParseResult argument)
    {
        Shell.Quit();
    }
}
