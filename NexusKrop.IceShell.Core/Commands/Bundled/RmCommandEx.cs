﻿// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Bundled;

using NexusKrop.IceShell.Core.Commands.Complex;

/// <summary>
/// Defines a command that does nothing. This command is to be used as a comment command.
/// </summary>
[ComplexCommand("rm")]
public class RmCommandEx : IComplexCommand
{
    public void Define(ComplexArgument argument)
    {
        argument.AddValue(new("comment", false));
    }

    public void Execute(ComplexArgumentParseResult argument)
    {
        // Does nothing.
    }
}