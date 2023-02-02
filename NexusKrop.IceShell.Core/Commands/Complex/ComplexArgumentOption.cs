namespace NexusKrop.IceShell.Core.Commands.Complex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public record ComplexArgumentOption
{
    public ComplexArgumentOption(string option, string? value = null)
    {
        Option = option;
        Value = value;
    }

    public string Option { get; set; }
    public string? Value { get; set; }
}
