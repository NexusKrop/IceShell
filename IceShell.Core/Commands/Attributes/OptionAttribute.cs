// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Core.Commands.Attributes;
using System;

/// <summary>
/// Defines a property as an option (also called switches).
/// </summary>
/// <remarks>
/// <para>
/// Options are specified with a forward slash (<c>/</c>) character then the unique identifying character. The character should not be forward slash,
/// and should be unique in the command it was registered with. The behaviour of using a forward slash as the identifying character, and using duplicate
/// identifying characters are undefined.
/// </para>
/// </remarks>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class OptionAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OptionAttribute"/> class.
    /// </summary>
    /// <param name="character">The character to use as the unique identifier (specifier) of this specific option.</param>
    /// <param name="hasValue">If <see langword="true"/>, this option requires a value; otherwise, this option does not accept values.</param>
    /// <param name="required">If <see langword="true"/>, this option is required. Required options are not recommended, because a value is more favoured in this context.</param>
    public OptionAttribute(char character, bool hasValue, bool required = false)
    {
        Character = character;
        HasValue = hasValue;
        Required = required;
    }

    /// <summary>
    /// Gets or sets the unique identifier (specifier) of this specific option.
    /// </summary>
    public char Character { get; set; }

    /// <summary>
    /// Gets or sets whether this option have an explicit value. If not, then the option is a <see cref="bool"/>, and
    /// the value of the option is whether the option presents in user input.
    /// </summary>
    public bool HasValue { get; set; }

    /// <summary>
    /// Gets or sets whether this option is required.
    /// </summary>
    public bool Required { get; set; }
}
