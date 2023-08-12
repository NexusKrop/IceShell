// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.FileSystem;

using NexusKrop.IceShell.Core.CLI;
using NexusKrop.IceShell.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

public static class PathSearcher
{
    public const char SHELL_SEPARATOR = '\\';

    private static readonly IReadOnlySet<string> PATHS;

    internal static bool UseCustomPathSystem { get; set; }

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

    public static string SystemToShell(string path)
    {
        if (!UseCustomPathSystem || OperatingSystem.IsWindows())
        {
            return path;
        }

        if (OperatingSystem.IsLinux() && path.StartsWith('/'))
        {
            return SystemToShell($"sys:{path}");
        }

        return path.Replace(Path.DirectorySeparatorChar, SHELL_SEPARATOR);
    }

    public static string ShellToSystem(string? path)
    {
        var actualPath = path ?? "";

        if (!UseCustomPathSystem && (!actualPath.Contains('\\') && !actualPath.StartsWith('@')))
        {
            if (path == $"~{Path.DirectorySeparatorChar}")
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            }

            return actualPath;
        }

        CheckPath(actualPath);

        if (path == "~\\")
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        }

        if (OperatingSystem.IsWindows())
        {
            return actualPath;
        }

        if (OperatingSystem.IsLinux() && actualPath.StartsWith("sys:\\"))
        {
            return ShellToSystem(actualPath.Remove(0, 4));
        }

        if (actualPath.StartsWith('~'))
        {
            var realDestination = actualPath.Remove(0, 1);
            return ShellToSystem(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                realDestination));
        }

        return actualPath.Replace(SHELL_SEPARATOR, Path.DirectorySeparatorChar);
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
