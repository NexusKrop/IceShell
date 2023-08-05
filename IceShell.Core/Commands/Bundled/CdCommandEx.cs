// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Bundled;

using global::IceShell.Core.Commands.Complex;
using NexusKrop.IceShell.Core.Commands.Complex;
using NexusKrop.IceShell.Core.Exceptions;
using NexusKrop.IceShell.Core.FileSystem;

/// <summary>
/// Defines a command that changes the current directory to the specified
/// directory.
/// </summary>
[ComplexCommand("cd", "Displays the name of or changes the current directory.")]
[CommandAlias("chdir")]
public class CdCommandEx : IComplexCommand
{
    public void Define(ComplexArgument argument)
    {
        argument.AddValue(new("destination", false));
    }

    public int Execute(ComplexArgumentParseResult argument, Shell shell)
    {
        if (argument.Values.Count == 0)
        {
            // Print current directory if no current directory is provided
            Console.WriteLine(PathSearcher.SystemToShell(Environment.CurrentDirectory));
            return 0;
        }

        var target = PathSearcher.ShellToSystem(argument.Values[0]!);

        if (!Directory.Exists(target))
        {
            throw ExceptionHelper.BadDirectory(target);
        }

        Shell.ChangeDirectory(target);

        return 0;
    }
}
