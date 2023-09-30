// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.FileSystem;

using global::IceShell.Core.CLI.Languages;
using global::IceShell.Core.Exceptions;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Text;

/// <summary>
/// Provides utilities for a custom path system.
/// </summary>
public static class PathSearcher
{
    /// <summary>
    /// The shell path separator.
    /// </summary>
    public const char SHELL_SEPARATOR = '\\';

    private static readonly IReadOnlySet<string> PATHS;

    internal const bool UseCustomPathSystem = false;

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

            AnsiConsole.MarkupLineInterpolated(FormattableStringFactory.Create("<red>{0}</red>", Languages.Get("shell_no_path")));
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
    /// Evaluate a path and expands all environment variables within.
    /// </summary>
    /// <param name="path">The path to expand.</param>
    /// <param name="lenient">If <see langword="true"/>, unknown or malformed environment variables will not cause <see cref="CommandFormatException"/> to be thrown.</param>
    /// <returns>The expanded path.</returns>
    public static string ExpandPath(ReadOnlySpan<char> path, bool lenient = false)
    {
        var inVariable = false;
        var builder = new StringBuilder();
        var varBuilder = new StringBuilder();

        for (int i = 0; i < path.Length; i++)
        {
            var x = path[i];

            if (x == '%')
            {
                if (!inVariable)
                {
                    // Begin an environment variable.
                    inVariable = true;
                    continue;
                }
                else
                {
                    // End an environment variable.
                    inVariable = false;

                    // Get variable name and clear builder to reuse.
                    var variableName = varBuilder.ToString();
                    varBuilder.Clear();

                    if (!string.IsNullOrEmpty(variableName))
                    {
                        var envVar = Environment.GetEnvironmentVariable(variableName);

                        if (envVar == null && !lenient)
                        {
                            throw ExceptionHelper.UnknownEnvironmentVariable(variableName);
                        }

                        builder.Append(envVar ?? string.Empty);
                        continue;
                    }

                    // If otherwise, this is hereby escaped.
                }
            }

            if (inVariable)
            {
                varBuilder.Append(x);
            }
            else
            {
                builder.Append(x);
            }
        }

        if (inVariable && !lenient)
        {
            throw ExceptionHelper.WithMessage("generic_env_variable_never_end");
        }

        return builder.ToString();
    }

    /// <summary>
    /// Determines whether the specified shell path points to the root of a file system.
    /// </summary>
    /// <param name="path">The path to check.</param>
    /// <remarks>
    /// <para>
    /// In Windows, each volume is considered a file system; in POSIX systems, there is only one file system being considered and that
    /// is the root volume.
    /// </para>
    /// </remarks>
    /// <returns><see langword="true"/> if the specified shell path points to the root of a file system; otherwise, <see langword="false"/>.</returns>
    public static bool IsRooted(ReadOnlySpan<char> path)
    {
        if (OperatingSystem.IsWindows())
        {
            return char.IsAsciiLetter(path[0]) && path[1] == ':' && path[2] == Path.DirectorySeparatorChar;
        }
        else
        {
            return path[0] == Path.DirectorySeparatorChar;
        }
    }

    /// <summary>
    /// Throws <see cref="CommandFormatException"/> if the specified path is not a valid path to a file.
    /// </summary>
    /// <param name="path">The path to check.</param>
    /// <exception cref="CommandFormatException">The specified path is invalid.</exception>
    public static void EnsureFileName(string path)
    {
        CheckPath(path);

        if (IsRooted(path))
        {
            return;
        }

        // This is some clean and precise ;)
        if (Array.Exists(Path.GetInvalidFileNameChars(), path.Contains))
        {
            throw new CommandFormatException(Languages.Get("generic_path_invalid"));
        }
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
        EnsureFileName(name);

        foreach (var path in PATHS)
        {
            if (!Directory.Exists(path))
            {
                continue;
            }

            var possible = GetSystemExecutableName(Path.Combine(path, name));

            if (!File.Exists(possible) || !FileUtility.IsExecutable(possible))
            {
                continue;
            }

            return possible;
        }

        return null;
    }
}
