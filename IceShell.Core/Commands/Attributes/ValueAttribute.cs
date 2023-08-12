namespace IceShell.Core.Commands.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ValueAttribute : Attribute
{
    public ValueAttribute(string name, bool required = true, int position = -1)
    {
        Name = name;
        Required = required;
        Position = position;
    }

    public string Name { get; set; }
    public bool Required { get; set; }
    public int Position { get; set; }
}
