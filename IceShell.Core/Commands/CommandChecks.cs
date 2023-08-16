namespace NexusKrop.IceShell.Core.Commands;

using global::IceShell.Core.CLI.Languages;
using NexusKrop.IceShell.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public static void DirectoryNotExists(string dir)
    {
        if (Directory.Exists(dir))
        {
            throw ExceptionHelper.WithName(Languages.Get("generic_directory_exists"), dir);
        }
    }
}
