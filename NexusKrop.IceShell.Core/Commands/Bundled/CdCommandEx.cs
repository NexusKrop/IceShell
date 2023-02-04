// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Bundled;

using NexusKrop.IceShell.Core.Commands.Complex;
using NexusKrop.IceShell.Core.Exceptions;
using NexusKrop.IceShell.Core.FileSystem;

/// <summary>
/// Defines a command that changes the current directory to the specified
/// directory.
/// </summary>
[ComplexCommand("cd")]
public class CdCommandEx : IComplexCommand
{
    public void Define(ComplexArgument argument)
    {
        argument.AddValue(new("destination", true));
    }

    public void Execute(ComplexArgumentParseResult argument)
    {
        var target = PathSearcher.ShellToSystem(argument.Values[0]!);

        if (!Directory.Exists(target))
        {
            throw new CommandFormatException(Messages.BadDirectory);
        }

        Shell.ChangeDirectory(target);
    }
}
