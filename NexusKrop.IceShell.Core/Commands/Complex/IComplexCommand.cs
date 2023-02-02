namespace NexusKrop.IceShell.Core.Commands.Complex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IComplexCommand
{
    void Define(ComplexArgument argument);

    void Execute(ComplexArgumentParseResult argument);
}
