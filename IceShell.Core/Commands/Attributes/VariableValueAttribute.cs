// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Core.Commands.Attributes;
using System;

/// <summary>
/// Specifies a command accepts variable amounts of values.
/// </summary>
/// <remarks>
/// <note type="warning">
/// A command that accepts variable amounts of values must also have a property of the <see cref="IList{T}"/> type, and the type of the list
/// must be <see cref="string"/>, and the property must also have the <see cref="VariableValueBufferAttribute"/> attribute.
/// </note>
/// <para>
/// There is no automatic parsing for commands with a variable amount of values, and the values
/// should be parsed at runtime.
/// </para>
/// </remarks>
[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public class VariableValueAttribute : Attribute
{
}
