// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Bundled;

using NexusKrop.IceShell.Core.Commands.Complex;
using NexusKrop.IceShell.Core.Exceptions;

/// <summary>
/// Defines a command that prints out the contents of a file.
/// </summary>
[ComplexCommand("type", "Displays the contents of a text file.")]
public class TypeCommandEx : IComplexCommand
{
    public void Define(ComplexArgument argument)
    {
        argument.AddValue("file", true);
    }

    public int Execute(ComplexArgumentParseResult argument)
    {
        if (argument.Values.Count != 1)
        {
            throw ExceptionHelper.WithName(ER.ComplexValueRequired, "file");
        }

        var val = argument.Values[0];

        if (string.IsNullOrWhiteSpace(val))
        {
            throw ExceptionHelper.WithName(ER.ComplexValueRequired, "file");
        }

        CommandChecks.FileExists(val);

        System.Console.WriteLine(File.ReadAllText(val));
        return 0;
    }
}
