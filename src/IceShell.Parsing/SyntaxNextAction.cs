// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Parsing;

/// <summary>
/// An enumeration of actions a segment does to the next segment.
/// </summary>
public enum SyntaxNextAction
{
    /// <summary>
    /// No action.
    /// </summary>
    None,
    /// <summary>
    /// Redirects the output of this segment to the next segment. Implicitly includes <see cref="IfSuccessOnly"/>.
    /// </summary>
    Redirect,
    /// <summary>
    /// Only executes next command of this segment is successful.
    /// </summary>
    IfSuccessOnly,
    /// <summary>
    /// Executes the next command regardless of the state of the last command.
    /// </summary>
    Continue
}
