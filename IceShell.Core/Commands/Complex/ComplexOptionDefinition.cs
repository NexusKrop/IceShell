// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Complex;
/// <summary>
/// Defines a complex command option argument.
/// </summary>
/// <remarks>
/// <para>
/// An option argument without a value is called a switch; the command checks for its presence in the
/// argument option set; otherwise, it is called an option, and is usually used to give optional or named
/// information or perference.
/// </para>
/// <para>
/// For a required value, we recommend you use a value argument whenever possible.
/// </para>
/// </remarks>
/// <param name="ShortName">The name of the option.</param>
/// <param name="HasValue">Whether or not this option has value.</param>
/// <param name="Required">Whether or not this option is required.</param>
public record struct ComplexOptionDefinition(char ShortName, bool HasValue, bool Required = false);
