// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Bundled;

using global::IceShell.Core.Commands.Attributes;
using NexusKrop.IceShell.Core.Commands.Complex;
using NexusKrop.IceShell.Core.Exceptions;

/// <summary>
/// Defines a command that prints out the contents of a file.
/// </summary>
[ComplexCommand("type", "Displays the contents of a text file.")]
public class TypeCommandEx : IComplexCommand
{
    [Value("file", position: 0)]
    public string? ArgFile { get; set; }

    public int Execute(ComplexArgumentParseResult argument, Shell shell)
    {
        if (ArgFile == null)
        {
            throw ExceptionHelper.WithName(ER.ComplexValueRequired, "file");
        }

        if (string.IsNullOrWhiteSpace(ArgFile))
        {
            throw ExceptionHelper.WithName(ER.ComplexValueRequired, "file");
        }

        CommandChecks.FileExists(ArgFile);

        System.Console.WriteLine(File.ReadAllText(ArgFile));
        return 0;
    }
}
