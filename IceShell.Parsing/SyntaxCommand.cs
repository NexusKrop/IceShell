namespace IceShell.Parsing;
using System.Collections.Generic;

/// <summary>
/// Represents a command specified by the user.
/// </summary>
public readonly record struct SyntaxCommand
{
    /// <summary>
    /// Initializes a new instance of <see cref="SyntaxCommand"/> class.
    /// </summary>
    /// <param name="name">The name of the command.</param>
    /// <param name="options">The list of options.</param>
    /// <param name="values">The list of values.</param>
    public SyntaxCommand(string name, ICollection<SyntaxOption> options, ICollection<SyntaxStatement> values)
    {
        Name = name;
        Options = options;
        Values = values; 
    }

    /// <summary>
    /// Gets the name of the command that were specified by the user.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the list of options that were specified by the user.
    /// </summary>
    public ICollection<SyntaxOption> Options { get; }

    /// <summary>
    /// Gets the list of users that were specified by the user.
    /// </summary>
    public ICollection<SyntaxStatement> Values { get; }
}