// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Bundled;

using global::IceShell.Core;
using global::IceShell.Core.Commands;
using global::IceShell.Core.Commands.Attributes;
using global::IceShell.Core.Commands.Complex;
using global::IceShell.Core.Exceptions;
using NexusKrop.IceShell.Core.Commands.Complex;
using NexusKrop.IceShell.Core.FileSystem;
using System;

/// <summary>
/// Creates a directory, or a tree of directories.
/// </summary>
[ComplexCommand("mkdir", "Creates a directory, or a tree of directories.")]
[CommandAlias("md")]
public class MkdirCommandEx : ICommand
{
    /// <summary>
    /// Gets or sets the name of the directory or that path syntax that represents the directory tree to create.
    /// </summary>
    [Value("directory", true, 0)]
    public string? DirectoryName { get; set; }

    /// <inheritdoc />
    public int Execute(IShell shell, ICommandExecutor executor)
    {
        var dir = PathSearcher.ShellToSystem(DirectoryName);

        CommandChecks.NothingExists(dir);

        try
        {
            Directory.CreateDirectory(dir);
        }
        catch (UnauthorizedAccessException)
        {
            throw ExceptionHelper.UnauthorizedWrite();
        }

        return 0;
    }
}
