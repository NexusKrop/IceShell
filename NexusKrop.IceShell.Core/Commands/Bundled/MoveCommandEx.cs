// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Bundled;

using NexusKrop.IceCube.Exceptions;
using NexusKrop.IceShell.Core.Commands.Complex;
using NexusKrop.IceShell.Core.Exceptions;
using NexusKrop.IceShell.Core.FileSystem;

/// <summary>
/// Defines a command that moves or renames a file.
/// </summary>
[ComplexCommand("move")]
public class MoveCommandEx : IComplexCommand
{
    public void Define(ComplexArgument argument)
    {
        argument.AddValue("source", true);
        argument.AddValue("destination", true);

        argument.AddOption('f', false);
    }

    public void Execute(ComplexArgumentParseResult argument)
    {
        var realSource = PathSearcher.ShellToSystem(argument.Values[0]!);
        var realDest = PathSearcher.ShellToSystem(argument.Values[1]!);

        Checks.FileExists(realSource);
        var force = argument.OptionPresents('f');

        if (File.Exists(realDest) && !force)
        {
            throw ExceptionHelper.WithName(Messages.MkdirFileExists, realDest);
        }

        File.Move(realSource, realDest, force);
    }
}
