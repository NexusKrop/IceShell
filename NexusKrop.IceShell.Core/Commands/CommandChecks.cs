namespace NexusKrop.IceShell.Core.Commands;

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
            throw ExceptionHelper.WithName(Messages.BadFile, file);
        }
    }

    /// <summary>
    /// Throws a <see cref="CommandFormatException"/> if the specified file does exist.
    /// </summary>
    /// <param name="file">The file to check.</param>
    public static void FileNotExists(string file)
    {
        if (File.Exists(file))
        {
            throw ExceptionHelper.WithName(Messages.MkdirFileExists, file);
        }
    }
}
