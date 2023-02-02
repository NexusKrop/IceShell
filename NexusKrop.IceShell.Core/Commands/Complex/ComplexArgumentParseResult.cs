namespace NexusKrop.IceShell.Core.Commands.Complex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class ComplexArgumentParseResult
{
    internal ComplexArgumentParseResult(IReadOnlyDictionary<char, string?> options, IReadOnlyList<string?> values)
    {
        Options = options;
        Values = values;
    }

    public IReadOnlyDictionary<char, string?> Options { get; set; }
    public IReadOnlyList<string?> Values { get; set; }
}
