// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Core.Commands.Complex;

/// <summary>
/// Defines an alias for commands.
/// </summary>
/// <remarks>
/// <para>
/// A command implementation type will be registered for each alias it defines as well as its own name.
/// </para>
/// </remarks>
[AttributeUsage(AttributeTargets.Class)]
public sealed class CommandAliasAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CommandAliasAttribute"/> class.
    /// </summary>
    /// <param name="alias">The alias.</param>
    public CommandAliasAttribute(string alias)
    {
        Alias = alias;
    }

    /// <summary>
    /// Gets or sets the alias.
    /// </summary>
    public string Alias { get; set; }
}
