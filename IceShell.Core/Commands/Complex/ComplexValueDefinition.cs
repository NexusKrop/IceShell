// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Complex;

/// <summary>
/// Defines a complex value argument.
/// </summary>
/// <remarks>
/// A complex value argument is an argument specified based on a particular order instead of a name. In
/// IceShell, the order of value arguments is the order of when they are defined through
/// <see cref="ComplexArgument.AddValue(ComplexValueDefinition)"/>.
/// </remarks>
/// <param name="Name">The name for reference.</param>
/// <param name="Required">Whether or not the argument is required.</param>
public record struct ComplexValueDefinition(string Name, bool Required = false);