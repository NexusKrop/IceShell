// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Core.Commands.Bundled;
using IceShell.Core.CLI.Languages;
using IceShell.Core.Commands.Attributes;
using Microsoft.Extensions.FileSystemGlobbing;
using NexusKrop.IceCube;
using NexusKrop.IceCube.Exceptions;
using NexusKrop.IceShell.Core.Commands;
using NexusKrop.IceShell.Core.Commands.Complex;
using NexusKrop.IceShell.Core.Exceptions;

using NexusKrop.IceShell.Core.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[VariableValue]
public abstract class DestinationFileCommandBase
{
    [VariableValueBuffer]
    public IReadOnlyList<string>? Buffer { get; set; }

    [Option('f', false)]
    public bool Force { get; set; }

    public abstract void DoOperation(string source, string destination);

    public int Execute(IShell shell, ICommandExecutor executor)
    {
        string? destination = null;

        if (Buffer == null || Buffer.Count < 2)
        {
            throw new CommandFormatException(Languages.RequiresValue(0));
        }

        var matcher = new Matcher();

        for (int i = 0; i < Buffer.Count; i++)
        {
            var str = PathSearcher.ShellToSystem(Buffer[i]);

            if (i == Buffer.Count - 1)
            {
                destination = Path.GetFullPath(PathSearcher.ShellToSystem(str));
                continue;
            }

            matcher.AddInclude(str);
        }

        var destIsDir = Directory.Exists(destination) || destination!.EndsWith(Path.DirectorySeparatorChar);

        if (destIsDir)
        {
            CommandChecks.DirectoryExists(destination);
        }

        var realFiles = matcher.GetResultsInFullPath(Directory.GetCurrentDirectory());
        var toCopy = new Dictionary<string, string>();

        foreach (var file in realFiles)
        {
            if (!File.Exists(file))
            {
                continue;
            }

            if (destIsDir)
            {
                toCopy.Add(file, Path.Combine(destination, Path.GetFileName(file)));
            }
            else
            {
                toCopy.Add(file, destination);
            }
        }

        if (!destIsDir && toCopy.Count > 2)
        {
            throw new CommandFormatException(Languages.CopyIsFileMore());
        }

        toCopy.ForEach(x => DoOperation(x.Key, x.Value));

        return 0;
    }
}
