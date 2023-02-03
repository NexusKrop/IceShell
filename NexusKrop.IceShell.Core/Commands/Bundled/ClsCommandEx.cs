namespace NexusKrop.IceShell.Core.Commands.Bundled;

using NexusKrop.IceShell.Core.Commands.Complex;
using System;

/// <summary>
/// Defines a command that clears the console screen.
/// </summary>
/// <seealso cref="Console.Clear()"/>
[ComplexCommand("cls")]
public class ClsCommandEx : IComplexCommand
{
    public void Define(ComplexArgument argument)
    {
        // No arguments needed to be defined here.
    }

    public void Execute(ComplexArgumentParseResult argument)
    {
        Console.Clear();
    }
}
