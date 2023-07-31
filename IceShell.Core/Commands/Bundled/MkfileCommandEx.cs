// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Bundled;

using NexusKrop.IceShell.Core.Commands.Complex;
using NexusKrop.IceShell.Core.Exceptions;
using NexusKrop.IceShell.Core.FileSystem;
using System;

/// <summary>
/// Defines a command that creates an empty file.
/// </summary>
[ComplexCommand("mkfile")]
public class MkfileCommandEx : IComplexCommand
{
    public void Define(ComplexArgument argument)
    {
        argument.AddValue("name", true);
    }

    public int Execute(ComplexArgumentParseResult argument)
    {
        var actual = PathSearcher.ShellToSystem(argument.Values[0]!);

        CommandChecks.FileNotExists(actual);
        CommandChecks.DirectoryNotExists(actual);

        try
        {
            // Create an absolutely empty file
            File.WriteAllBytes(actual, Array.Empty<byte>());
        }
        catch (UnauthorizedAccessException)
        {
            throw new CommandFormatException(Messages.FileUnauthorizedCreate);
        }

        return 0;
    }
}
