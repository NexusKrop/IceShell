// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Bundled;

using global::IceShell.Core.Commands.Attributes;
using NexusKrop.IceShell.Core.Commands.Complex;

/// <summary>
/// Defines a command that does nothing and always succeeds. This command is to be used as a comment command.
/// </summary>
[ComplexCommand("rm")]
[GreedyString]
public class RmCommandEx : IComplexCommand
{
    [Value("comment", position: 0)]
    public string? Comment { get; set; }

    public int Execute(ComplexArgumentParseResult argument, Shell shell)
    {
        return 0;
    }
}
