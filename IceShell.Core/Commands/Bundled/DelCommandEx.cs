// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Bundled;

using global::IceShell.Core;
using global::IceShell.Core.Commands.Attributes;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using NexusKrop.IceCube;
using NexusKrop.IceCube.Exceptions;
using NexusKrop.IceShell.Core.Commands.Complex;
using NexusKrop.IceShell.Core.Exceptions;
using NexusKrop.IceShell.Core.FileSystem;

/// <summary>
/// Defines a command that deletes a file.
/// </summary>
[ComplexCommand("del", "Deletes a file.")]
public class DelCommandEx : IComplexCommand
{
    [Value("target", position: 0)]
    public string? Target { get; set; }

    private static void DeleteFileCommit(string file)
    {
        try
        {
            File.Delete(file);
        }
        catch (UnauthorizedAccessException)
        {
            throw new CommandFormatException(Messages.FileUnauthorized);
        }
    }

    public int Execute(ComplexArgumentParseResult argument, IShell shell)
    {
        var pattern = Target;

        if (string.IsNullOrWhiteSpace(pattern))
        {
            throw ExceptionHelper.WithName(ER.ComplexValueRequired, '0');
        }

        var searchDir = Path.GetDirectoryName(PathSearcher.ShellToSystem(pattern!));

        if (string.IsNullOrWhiteSpace(searchDir))
        {
            searchDir = Directory.GetCurrentDirectory();
        }

        var targets = new List<string>(argument.Values.Count);

        // Match via file glob
        var matcher = new Matcher();
        matcher.AddInclude(PathSearcher.ShellToSystem(pattern));

        var matchingResult = matcher.Execute(new DirectoryInfoWrapper(new DirectoryInfo(searchDir ?? Environment.CurrentDirectory)));

        matchingResult.Files.ForEach(x =>
        {
            targets.Add(x.Path);
        });

        var failure = false;

        if (!targets.Any())
        {
            System.Console.WriteLine("No files found for pattern \"{0}\".", PathSearcher.ShellToSystem(pattern));

#if DEBUG
            System.Console.WriteLine(pattern);
#endif

            return 1;
        }

        if (targets.Count > 1)
        {
            foreach (var target in targets)
            {
                if (!File.Exists(target))
                {
                    failure = true;
                    System.Console.WriteLine("ERROR: File \"{0}\" does not exist or no permission to access it", target);
                    continue;
                }

                DeleteFileCommit(target);
            }

            return failure ? 1 : 0;
        }
        else
        {
            var target = targets[0];

            CommandChecks.FileExists(target);

            // Reserved for future use
            DeleteFileCommit(target);

        }

        return 0;
    }
}
