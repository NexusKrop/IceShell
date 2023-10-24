// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

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
    /// Invocation of an internal command.
    /// </summary>
    InternalCommand,
    /// <summary>
    /// Invocation of an external command.
    /// </summary>
    ExternalCommand,
}
