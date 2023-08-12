// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Complex;
using System;

/// <summary>
/// Specifies that the class attributed is a complex command.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class ComplexCommandAttribute : Attribute
{
    public ComplexCommandAttribute(string name, string? description = null)
    {
        Name = name;
        Description = description;
    }

    public string Name { get; set; }
    public string? Description { get; set; }
    public string? CustomUsage { get; set; }
}
