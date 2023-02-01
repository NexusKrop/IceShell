namespace NexusKrop.IceShell.Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class CommandAttribute : Attribute
{
    public CommandAttribute(string name, int numArgs)
    {
        Name = name;
        NumArgs = numArgs;
    }

    public string Name { get; set; }
    public int NumArgs { get; set; }
}
