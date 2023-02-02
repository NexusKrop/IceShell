namespace NexusKrop.IceShell.Core.Commands.Complex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[AttributeUsage(AttributeTargets.Class)]
public sealed class ComplexCommandAttribute : Attribute
{
    public ComplexCommandAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
}
