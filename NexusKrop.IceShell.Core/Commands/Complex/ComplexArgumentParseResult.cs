// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Complex;
using System.Collections.Generic;

/// <summary>
/// Represents the parse results of a complex argument set. This class cannot be inherited.
/// </summary>
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
