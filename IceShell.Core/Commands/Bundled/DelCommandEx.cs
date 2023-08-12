// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Bundled;

using global::IceShell.Core;
using global::IceShell.Core.CLI.Languages;
using global::IceShell.Core.Commands.Attributes;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using NexusKrop.IceCube;
using NexusKrop.IceCube.Exceptions;
using NexusKrop.IceShell.Core.CLI;
using NexusKrop.IceShell.Core.Commands.Complex;
using NexusKrop.IceShell.Core.Exceptions;
using NexusKrop.IceShell.Core.FileSystem;

/// <summary>
/// Defines a command that deletes a file.
/// </summary>
[ComplexCommand("del", "Deletes one or more files.")]
[VariableValue]
public class DelCommandEx : IComplexCommand
{
    [VariableValueBuffer]
    public IReadOnlyList<string>? Targets { get; set; }

    [Option('P', false)]
    public bool Prompt { get; set; }

    private void DeleteFileCommit(string file)
    {
        try
        {
            if (Prompt && !ConsoleAsk.YesNo(Languages.DelPrompt(file)))
            {
                return;
            }

            File.Delete(file);
        }
        catch (UnauthorizedAccessException)
        {
            throw new CommandFormatException(Languages.UnauthorizedFile(file));
        }
    }

    public int Execute(ComplexArgumentParseResult argument, IShell shell)
    {
        if (Targets == null || !Targets.Any())
        {
            throw ExceptionHelper.WithName(ER.ComplexValueRequired, '0');
        }

        string searchDir;

        if (Targets.Count == 1)
        {
            searchDir = Path.GetDirectoryName(PathSearcher.ShellToSystem(Targets[0]))!;
        }
        else
        {
            searchDir = Directory.GetCurrentDirectory();
        }

        if (string.IsNullOrWhiteSpace(searchDir))
        {
            searchDir = Directory.GetCurrentDirectory();
        }

        var targets = new List<string>(argument.Values.Count);

        // Match via file glob
        var matcher = new Matcher();

        Targets.ForEach(x => matcher.AddInclude(PathSearcher.ShellToSystem(x)));

        var matchingResult = matcher.Execute(new DirectoryInfoWrapper(new DirectoryInfo(searchDir ?? Environment.CurrentDirectory)));

        matchingResult.Files.ForEach(x =>
        {
            targets.Add(x.Path);
        });

        var failure = false;

        if (!targets.Any())
        {
            Console.WriteLine("No files found for the selected patterns");

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
