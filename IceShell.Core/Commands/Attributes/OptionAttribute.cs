// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Core.Commands.Attributes;
using System;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class OptionAttribute : Attribute
{
    public OptionAttribute(char character, bool hasValue, bool required = false)
    {
        Character = character;
        HasValue = hasValue;
        Required = required;
    }

    public char Character { get; set; }

    /// <summary>
    /// Gets or sets whether this option have an explicit value. If not, then the option is a <see cref="bool"/>, and
    /// the value of the option is whether the option presents in user input.
    /// </summary>
    public bool HasValue { get; set; }

    public bool Required { get; set; }
}
