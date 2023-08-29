// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Core.Commands.Attributes;
using System;

/// <summary>
/// Defines a property as an value.
/// </summary>
/// <remarks>
/// We recommends against specifying two or more command with the same position value is undefined,
/// because the consequences are undefined and could result in an unusable command.
/// </remarks>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ValueAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ValueAttribute"/> class.
    /// </summary>
    /// <param name="name">The reference name of the value.</param>
    /// <param name="required">If <see langword="true"/>, this value is required.</param>
    /// <param name="position">The position of this value in a command.</param>
    public ValueAttribute(string name, bool required = true, int position = -1)
    {
        Name = name;
        Required = required;
        Position = position;
    }

    /// <summary>
    /// Gets or sets the reference name of the value.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets whether this value is required.
    /// </summary>
    public bool Required { get; set; }

    /// <summary>
    /// Gets or sets the position of this value.
    /// </summary>
    public int Position { get; set; }
}
