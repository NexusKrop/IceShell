// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Core.Commands;

using IceShell.Core.CLI.Languages;
using IceShell.Core.Exceptions;

/// <summary>
/// Provides methods to throw exceptions when certain conditions are met or are not met.
/// </summary>
public static class CommandChecks
{
    /// <summary>
    /// Throws a <see cref="CommandFormatException"/> if the specified file does not exist.
    /// </summary>
    /// <param name="file">The file to check.</param>
    public static void FileExists(string file)
    {
        if (!File.Exists(file))
        {
            throw ExceptionHelper.WithName(Languages.Get("generic_file_not_found"), file);
        }
    }

    /// <summary>
    /// Throws a <see cref="CommandFormatException"/> if the specified directory does exist.
    /// </summary>
    /// <param name="directory">The directory to check.</param>
    public static void DirectoryExists(string? directory)
    {
        if (string.IsNullOrWhiteSpace(directory) || !Directory.Exists(directory))
        {
            throw ExceptionHelper.WithName(Languages.Get("generic_directory_not_found"), directory);
        }
    }


    /// <summary>
    /// Throws a <see cref="CommandFormatException"/> if a file or directory with the specified name exists.
    /// </summary>
    /// <param name="name"></param>
    public static void NothingExists(string name)
    {
        FileNotExists(name);
        DirectoryNotExists(name);
    }

    /// <summary>
    /// Throws a <see cref="CommandFormatException"/> if the specified file does exist.
    /// </summary>
    /// <param name="file">The file to check.</param>
    public static void FileNotExists(string file)
    {
        if (File.Exists(file))
        {
            throw ExceptionHelper.WithName(Languages.Get("generic_file_exists"), file);
        }
    }

    /// <summary>
    /// Throws a <see cref="CommandFormatException"/> if the specified directory does not exist.
    /// </summary>
    /// <param name="directory">The directory to check.</param>
    public static void DirectoryNotExists(string directory)
    {
        if (Directory.Exists(directory))
        {
            throw ExceptionHelper.WithName(Languages.Get("generic_directory_exists"), directory);
        }
    }
}
