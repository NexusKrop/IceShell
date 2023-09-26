namespace IceShell.Core.Commands;

using NexusKrop.IceCube.Util;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Represents a collection of batches.
/// </summary>
public class BatchLineCompound : IEnumerable<BatchLine>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BatchLineCompound"/> class.
    /// </summary>
    /// <param name="lines">A list containing the batch line instances.</param>
    public BatchLineCompound(IList<BatchLine> lines)
    {
        Lines = lines;
    }

    /// <summary>
    /// Gets a list containing the batch line instances.
    /// </summary>
    public IList<BatchLine> Lines { get; }

    /// <inheritdoc />
    public IEnumerator<BatchLine> GetEnumerator()
    {
        return Lines.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)Lines).GetEnumerator();
    }

    public static BatchLineCompound Single(BatchLine line)
    {
        return new BatchLineCompound(Arrays.From(line));
    }

    public static BatchLineCompound Empty()
    {
        return new BatchLineCompound(Array.Empty<BatchLine>());
    }
}