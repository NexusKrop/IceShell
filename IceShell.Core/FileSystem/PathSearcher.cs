// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.FileSystem;

using global::IceShell.Core.CLI.Languages;
using NexusKrop.IceShell.Core.CLI;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

/// <summary>
/// Provides utilities for a custom path system.
/// </summary>
public static class PathSearcher
{
    /// <summary>
    /// The shell path seperator.
    /// </summary>
    public const char SHELL_SEPARATOR = '\\';

    private static readonly IReadOnlySet<string> PATHS;

    internal static bool UseCustomPathSystem { get; set; }

#pragma warning disable S3963
    // This is a big chunk of complex code that SonarLint is too dumb to detect.
    static PathSearcher()
#pragma warning restore S3963
    {
        var result = new HashSet<string>();

        var path = Environment.GetEnvironmentVariable("PATH");

        if (path == null)
        {
            // Doing so will cause the shell cannot find anything in PATH because
            // there was no PATH.

            ConsoleOutput.WriteLineColour(Languages.Get("shell_no_path"), ConsoleColor.Yellow);
            Console.WriteLine();

            // 2023/08/17, WithLithum - not sure what I was doing here but
            // I'll keep this comment in case I remember.

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

    /// <summary>
    /// Ensures that the specified shell path is invalid.
    /// </summary>
    /// <param name="path">The path to check.</param>
    /// <exception cref="FormatException">The shell path is invalid.</exception>
    public static void CheckPath(string path)
    {
        if (path.Contains('/'))
        {
            throw new FormatException(Languages.Get("generic_path_invalid"));
        }
    }

    /// <summary>
    /// Determines whether the specified shell path points to the root of a drive or the file system
    /// (depending on the operating system).
    /// </summary>
    /// <param name="path">The path to check.</param>
    /// <returns><see langword="true"/> if the specified shell path points to the root of a drive or the file system; otherwise, <see langword="false"/>.</returns>
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
        foreach (var ch in Path.GetInvalidFileNameChars())
        {
            if (path.Contains(ch))
            {
                throw new FormatException(Languages.Get("generic_path_invalid"));
            }
        }
#pragma warning restore S3267
    }

    /// <summary>
    /// Converts a platform-dependent system path to its shell path equivalent.
    /// </summary>
    /// <param name="path">The system path to convert from.</param>
    /// <returns>The shell path equivalent of the specified system path.</returns>
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

    /// <summary>
    /// Converts a shell path to its platform-dependent equivalent.
    /// </summary>
    /// <param name="path">The shell path to convert from.</param>
    /// <returns>The platform-dependent equivalent of the specified shell path.</returns>
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

    /// <summary>
    /// Gets the name of the executable file of the specified name.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns>The name of the executable file.</returns>
    public static string GetSystemExecutableName(string name)
    {
        if (OperatingSystem.IsWindows() && !Path.HasExtension(name))
        {
            return string.Format("{0}.exe", name);
        }

        return name;
    }

    /// <summary>
    /// Searches for an executable in <c>PATH</c>.
    /// </summary>
    /// <param name="name">The name of the executable to search.</param>
    /// <returns>The first executable found.</returns>
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
