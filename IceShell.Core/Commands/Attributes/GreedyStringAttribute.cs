// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Core.Commands.Attributes;
using System;

/// <summary>
/// Specifies the string value in the last position of a command extends to the end of the line, ignoring
/// spaces.
/// </summary>
/// <remarks>
/// <para>
/// Given a command is defined as a greedy string command, a command like this:
/// <code language="none">
/// greedy arg1 arg2 arg3 and spaces
/// </code>
/// The arguments will be parsed to this:
/// <list type="bullet">
///     <item>arg1</item>
///     <item>arg2</item>
///     <item>arg3 and spaces</item>
/// </list>
/// </para>
/// </remarks>
[AttributeUsage(AttributeTargets.Class)]
public class GreedyStringAttribute : Attribute
{
}
