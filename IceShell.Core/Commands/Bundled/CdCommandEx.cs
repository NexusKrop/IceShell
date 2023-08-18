// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Bundled;

using global::IceShell.Core;
using global::IceShell.Core.Commands;
using global::IceShell.Core.Commands.Attributes;
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
[GreedyString]
public class CdCommandEx : ICommand
{
    [Value("destination", position: 0)]
    public string? Destination { get; set; }

    public int Execute(IShell shell, ICommandExecutor executor)
    {
        if (string.IsNullOrWhiteSpace(Destination))
        {
            // Print current directory if no current directory is provided
            Console.WriteLine(PathSearcher.SystemToShell(Environment.CurrentDirectory));
            return 0;
        }

        var target = PathSearcher.ShellToSystem(Destination ?? "");

        if (!Directory.Exists(target))
        {
            throw ExceptionHelper.BadDirectory(target);
        }

        Shell.ChangeDirectory(target);

        return 0;
    }
}
