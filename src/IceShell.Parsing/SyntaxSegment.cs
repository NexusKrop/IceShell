// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Parsing;

/// <summary>
/// Represents a segment in a command compound.
/// </summary>
public record SyntaxSegment
{
    /// <summary>
    /// Initializes a new instance of <see cref="SyntaxSegment"/> class as a command segment.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="nextAction">The action to perform when executing the next segment.</param>
    public SyntaxSegment(SyntaxCommand command, SyntaxNextAction nextAction = SyntaxNextAction.None)
    {
        Command = command;
        Type = SegmentType.Command;
        NextAction = nextAction;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="SyntaxSegment"/> class as a file segment.
    /// </summary>
    /// <param name="file">The file.</param>
    /// <param name="nextAction">The action to perform when executing the next segment.</param>
    /// <param name="additionalStatements">Additional statements to pass when executing the file.</param>
    public SyntaxSegment(string file, SyntaxNextAction nextAction = SyntaxNextAction.None, IEnumerable<SyntaxStatement>? additionalStatements = null)
    {
        File = file;
        Type = SegmentType.File;
        NextAction = nextAction;
        FileStatements = additionalStatements;
    }

    /// <summary>
    /// Gets or sets the command of this instance.
    /// </summary>
    /// <value>
    /// The command of this instance. The value should be ignored if <see cref="Type"/> is not <see cref="SegmentType.Command"/>.
    /// </value>
    public SyntaxCommand? Command { get; set; }

    /// <summary>
    /// Gets or sets the type of the command.
    /// </summary>
    public SegmentType Type { get; set; }

    /// <summary>
    /// Gets or sets the file identifier of this instance.
    /// </summary>
    /// <value>
    /// The file identifier of this instance. The value should be ignored if <see cref="Type"/> is not <see cref="SegmentType.File"/>.
    /// </value>
    public string? File { get; set; }

    /// <summary>
    /// Get or sets the additional statements to supply to the file.
    /// </summary>
    public IEnumerable<SyntaxStatement>? FileStatements { get; set; }

    /// <summary>
    /// Gets or sets the action to perform when executing the next segment.
    /// </summary>
    public SyntaxNextAction NextAction { get; set; } = SyntaxNextAction.None;
}
