// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Parsing;

/// <summary>
/// Represents a segment in a command compound.
/// </summary>
public record SyntaxSegment
{
    /// <summary>
    /// Initializes a new instance of <see cref="SyntaxSegment"/> class as internal command invocation segment.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="nextAction">The action to perform when executing the next segment.</param>
    public SyntaxSegment(SyntaxCommandInvocation command, SyntaxNextAction nextAction = SyntaxNextAction.None)
    {
        InternalCommand = command;
        Type = SegmentType.InternalCommand;
        NextAction = nextAction;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="SyntaxSegment"/> class as external command invocation segment.
    /// </summary>
    /// <param name="externalCommand">The external command reference.</param>
    /// <param name="nextAction">The action to perform when executing the next segment.</param>
    public SyntaxSegment(ExternalCommandRef externalCommand, SyntaxNextAction nextAction = SyntaxNextAction.None)
    {
        ExternalCommand = externalCommand;
        Type = SegmentType.ExternalCommand;
        NextAction = nextAction;
    }

    /// <summary>
    /// Gets or sets the internal command invocation.
    /// </summary>
    /// <value>
    /// The command of this instance. The value should be ignored if <see cref="Type"/> is not <see cref="SegmentType.InternalCommand"/>.
    /// </value>
    public SyntaxCommandInvocation? InternalCommand { get; set; }

    /// <summary>
    /// Gets or sets the external command reference.
    /// </summary>
    public ExternalCommandRef ExternalCommand { get; set; }

    /// <summary>
    /// Gets or sets the type of the command.
    /// </summary>
    public SegmentType Type { get; set; }

    /// <summary>
    /// Gets or sets the file identifier of this instance.
    /// </summary>
    /// <value>
    /// The file identifier of this instance. The value should be ignored.
    /// </value>
    [Obsolete("Use ExternalCommand for external commands.")]
    public string? File { get; set; }

    /// <summary>
    /// Gets or sets the action to perform when executing the next segment.
    /// </summary>
    public SyntaxNextAction NextAction { get; set; } = SyntaxNextAction.None;
}
