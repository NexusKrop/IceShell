// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Core.Commands.Attributes;
using System;

/// <summary>
/// Specifies that variable amount of values should be put into this property.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class VariableValueBufferAttribute : Attribute
{
}
