// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Core.Commands;

using IceShell.Core.Api;
using IceShell.Core.Exceptions;

/// <summary>
/// Provides methods to throw exceptions when certain conditions are met or are not met.
/// </summary>
public static class CommandChecks
{
    /// <summary>
    /// Interrupts the command execution if the specified file does not exist.
    /// </summary>
    /// <param name="file">The file to check.</param>
    public static void FileExists(string? file)
    {
        if (!File.Exists(file))
        {
            throw new CommandInterruptException(CommandResult.WithBadFile(file ?? "<null>"));
        }
    }

    /// <summary>
    /// Interrupts the command execution if the specified directory does exist.
    /// </summary>
    /// <param name="directory">The directory to check.</param>
    public static void DirectoryExists(string? directory)
    {
        if (string.IsNullOrWhiteSpace(directory) || !Directory.Exists(directory))
        {
            throw new CommandInterruptException(CommandResult.WithBadDirectory(directory ?? "<null>"));
        }
    }


    /// <summary>
    /// Interrupts the command execution if a file or directory with the specified name exists.
    /// </summary>
    /// <param name="name"></param>
    public static void NothingExists(string name)
    {
        FileNotExists(name);
        DirectoryNotExists(name);
    }

    /// <summary>
    /// Interrupts the command execution if the specified file does exist.
    /// </summary>
    /// <param name="file">The file to check.</param>
    public static void FileNotExists(string file)
    {
        if (File.Exists(file))
        {
            throw new CommandInterruptException(CommandResult.WithExistingFile(file));
        }
    }

    /// <summary>
    /// Interrupts the command execution if the specified directory does not exist.
    /// </summary>
    /// <param name="directory">The directory to check.</param>
    public static void DirectoryNotExists(string directory)
    {
        if (Directory.Exists(directory))
        {
            throw new CommandInterruptException(CommandResult.WithExistingDirectory(directory));
        }
    }
}
