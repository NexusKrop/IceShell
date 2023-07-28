// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Bundled;

using NexusKrop.IceShell.Core.Commands.Complex;
using System;

/// <summary>
/// Defines a command that echoes or displays the specified text.
/// </summary>
[ComplexCommand("echo", "Displays messages.")]
public class EchoCommandEx : IComplexCommand
{
    public void Define(ComplexArgument argument)
    {
        argument.AddValue(new("message", true));
        argument.MakeGreedy();
    }

    public int Execute(ComplexArgumentParseResult argument)
    {
        Console.WriteLine(argument.Values[0]);
        return 0;
    }
}
