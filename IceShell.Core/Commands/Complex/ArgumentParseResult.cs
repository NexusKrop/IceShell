// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Complex;
using System.Collections.Generic;

/// <summary>
/// Represents the parse results of a complex argument set. This class cannot be inherited.
/// </summary>
public sealed class ArgumentParseResult
{
    internal ArgumentParseResult()
    {
    }

    /// <summary>
    /// Gets the list of options resolved.
    /// </summary>
    public IDictionary<ComplexOptionDefinition, string?> Options { get; } = new Dictionary<ComplexOptionDefinition, string?>();

    /// <summary>
    /// Gets the list of values resolved.
    /// </summary>
    public IDictionary<ComplexValueDefinition, string?> Values { get; } = new Dictionary<ComplexValueDefinition, string?>();

    /// <summary>
    /// Gets the list of all variable values resolved.
    /// </summary>
    public IList<string?> VariableValues { get; } = new List<string?>();

    /// <summary>
    /// Appends an option to this instance.
    /// </summary>
    /// <param name="option">The definition of the option.</param>
    /// <param name="value">The value of the option.</param>
    /// <returns>This instance (for chaining purposes).</returns>
    public ArgumentParseResult Option(ComplexOptionDefinition option, string? value)
    {
        Options[option] = value;
        return this;
    }

    /// <summary>
    /// Appends a value argument to this instance.
    /// </summary>
    /// <param name="valueDef">The definition of the value argument.</param>
    /// <param name="value">The value of the argument.</param>
    /// <returns>This instance (for chaining purposes).</returns>
    public ArgumentParseResult Value(ComplexValueDefinition valueDef, string? value)
    {
        Values[valueDef] = value;
        return this;
    }
}
