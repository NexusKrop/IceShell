namespace IceShell.Parsing;

using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Represents a set of commands.
/// </summary>
public record SyntaxCompound : IEnumerable<SyntaxSegment>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SyntaxCompound"/> class.
    /// </summary>
    /// <param name="segments">The segments.</param>
    public SyntaxCompound(IList<SyntaxSegment> segments)
    {
        Segments = segments;
    }

    /// <summary>
    /// Gets the segments of this compound.
    /// </summary>
    public IList<SyntaxSegment> Segments { get; }

    /// <inheritdoc />
    public IEnumerator<SyntaxSegment> GetEnumerator()
    {
        return Segments.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)Segments).GetEnumerator();
    }
}
