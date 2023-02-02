namespace NexusKrop.IceShell.Core.FileSystem;

using NexusKrop.IceShell.Core.CLI;
using NexusKrop.IceShell.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class PathSearcher
{
    public const char SHELL_SEPARATOR = '\\';

    private static readonly IReadOnlySet<string> PATHS;

#pragma warning disable S3963
    // This is a big chunk of complex code that unfortunealy SonarLint is too dumb, and cannot detect that.
    static PathSearcher()
#pragma warning restore S3963
    {
        var result = new HashSet<string>();

        var path = Environment.GetEnvironmentVariable("PATH");

        if (path == null)
        {
            // Doing so will cause the shell cannot find anything in PATH because
            // there was no PATH.

            ConsoleOutput.WriteLineColour(Messages.NoPathWarning1, ConsoleColor.Yellow);
            Console.WriteLine();

            // We will still set PATHS to a
        }
        else
        {
            var splitted = path.Split(Path.PathSeparator);

            foreach (var p in splitted)
            {
                result.Add(p);
            }
        }

        PATHS = result.ToImmutableHashSet();
    }

    public static void CheckPath(string path)
    {
        if (path.Contains('/'))
        {
            throw new FormatException(ER.InvalidPath);
        }
    }

    public static bool IsRootedShell(string path)
    {
        if (OperatingSystem.IsWindows())
        {
            return Path.IsPathRooted(path);
        }
        else
        {
            return path.StartsWith(SHELL_SEPARATOR);
        }
    }

    public static void CheckFileName(string path)
    {
        CheckPath(path);

        if (IsRootedShell(path))
        {
            return;
        }

#pragma warning disable S3267
        // I do not see if LINQ is here anywhere near clear and concise.
        foreach (var chara in Path.GetInvalidFileNameChars())
        {
            if (path.Contains(chara))
            {
                throw new FormatException(ER.InvalidPath);
            }
        }
#pragma warning restore S3267
    }

    public static string ShellToSystem(string path)
    {
        CheckPath(path);

        if (OperatingSystem.IsWindows())
        {
            return path;
        }

        return path.Replace(SHELL_SEPARATOR, Path.DirectorySeparatorChar);
    }

    public static string GetSystemExecutableName(string name)
    {
        if (OperatingSystem.IsWindows() && !Path.HasExtension(name))
        {
            return string.Format("{0}.exe", name);
        }

        return name;
    }

    public static string? SearchExecutable(string name)
    {
        CheckFileName(name);

        foreach (var path in PATHS)
        {
            if (!Directory.Exists(path))
            {
                continue;
            }

            var possible = GetSystemExecutableName(Path.Combine(path, name));

            if (!File.Exists(possible) || !FileUtil.IsExecutable(possible))
            {
                continue;
            }

            return possible;
        }

        return null;
    }
}
