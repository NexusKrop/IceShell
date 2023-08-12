// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Complex;
using System.Collections.Generic;

/// <summary>
/// Represents the parse results of a complex argument set. This class cannot be inherited.
/// </summary>
public sealed class ComplexArgumentParseResult
{
    internal ComplexArgumentParseResult()
    {
    }

    public IDictionary<ComplexOptionDefinition, string?> Options { get; } = new Dictionary<ComplexOptionDefinition, string?>();
    public IDictionary<ComplexValueDefinition, string?> Values { get; } = new Dictionary<ComplexValueDefinition, string?>();
    public IList<string?> VariableValues { get; } = new List<string?>();

    public ComplexArgumentParseResult Option(ComplexOptionDefinition option, string? value)
    {
        Options[option] = value;
        return this;
    }

    public ComplexArgumentParseResult Value(ComplexValueDefinition value, string? obj)
    {
        Values[value] = obj;
        return this;
    }
}
