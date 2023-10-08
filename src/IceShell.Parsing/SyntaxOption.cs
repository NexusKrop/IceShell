// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Parsing;

/// <summary>
/// Represents an option specified by the user.
/// </summary>
public readonly record struct SyntaxOption
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SyntaxOption"/> structure.
    /// </summary>
    /// <param name="identifier">The identifier.</param>
    /// <param name="value">The value.</param>
    public SyntaxOption(char identifier, string? value)
    {
        Identifier = identifier;
        Value = value;
    }

    /// <summary>
    /// Gets the identifier of this option.
    /// </summary>
    public char Identifier { get; }

    /// <summary>
    /// Gets the value of this option.
    /// </summary>
    /// <value>
    /// The value specified; <see langword="null"/> if there were no value.
    /// </value>
    public string? Value { get; }
}
