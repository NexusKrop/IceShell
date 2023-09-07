namespace IceShell.Parsing;

/// <summary>
/// An enumeration of the available types of <see cref="SyntaxSegment"/>.
/// </summary>
public enum SegmentType
{
    /// <summary>
    /// An empty <see cref="SyntaxSegment"/> with no file identifier nor a command.
    /// </summary>
    None,
    /// <summary>
    /// A <see cref="SyntaxSegment"/> with a command.
    /// </summary>
    Command,
    /// <summary>
    /// A <see cref="SyntaxSegment"/> with a file.
    /// </summary>
    File
}
