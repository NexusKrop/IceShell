// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Bundled;

using global::IceShell.Core;
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
    // TODO redefine
    //public void Define(ComplexArgument argument)
    //{
    //    argument.AddValue("source", true);
    //    argument.AddValue("destination", true);

    //    argument.AddOption('f', false);
    //}

    public int Execute(ComplexArgumentParseResult argument, IShell shell)
    {
        //var realSource = PathSearcher.ShellToSystem(argument.Values[0]!);
        //var realDest = PathSearcher.ShellToSystem(argument.Values[1]!);
        //var force = argument.OptionPresents('f');

        //CommandChecks.FileExists(realSource);
        //CommandChecks.DirectoryNotExists(realDest);

        //if (File.Exists(realDest) && !force)
        //{
        //    throw ExceptionHelper.WithName(Messages.MkdirFileExists, realDest);
        //}

        //try
        //{
        //    File.Move(realSource, realDest, force);
        //}
        //catch (UnauthorizedAccessException)
        //{
        //    throw new CommandFormatException(Messages.FileUnauthorized);
        //}
        //catch (Exception ex)
        //{
        //    System.Console.WriteLine(ex);
        //    return 1;
        //}

        return 0;
    }
}
