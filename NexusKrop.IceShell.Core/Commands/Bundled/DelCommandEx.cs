// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Bundled;

using NexusKrop.IceCube.Exceptions;
using NexusKrop.IceShell.Core.Commands.Complex;
using NexusKrop.IceShell.Core.Exceptions;
using NexusKrop.IceShell.Core.FileSystem;

/// <summary>
/// Defines a command that deletes a file.
/// </summary>
[ComplexCommand("del", "Deletes a file.")]
public class DelCommandEx : IComplexCommand
{
    public void Define(ComplexArgument argument)
    {
        argument.AddValue("target", true);
    }

    private static void DeleteFileCommit(string file)
    {
        try
        {
            File.Delete(file);
        }
        catch (UnauthorizedAccessException)
        {
            throw new CommandFormatException(Messages.FileUnauthorized);
        }
    }

    public int Execute(ComplexArgumentParseResult argument)
    {
        var target = PathSearcher.ShellToSystem(argument.Values[0]!);

        CommandChecks.FileExists(target);

        // Reserved for future use
        DeleteFileCommit(target);

        return 0;
    }
}
