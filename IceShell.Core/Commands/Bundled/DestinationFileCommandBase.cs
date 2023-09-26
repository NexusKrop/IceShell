// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Core.Commands.Bundled;
using IceShell.Core.CLI.Languages;
using IceShell.Core.Commands;
using IceShell.Core.Commands.Attributes;
using IceShell.Core.Exceptions;
using Microsoft.Extensions.FileSystemGlobbing;
using NexusKrop.IceCube;
using NexusKrop.IceCube.Exceptions;
using NexusKrop.IceShell.Core.Commands.Complex;
using NexusKrop.IceShell.Core.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Provides a base class for file operation commands that involves multiple sources and a single destination.
/// </summary>
[VariableValue]
public abstract class DestinationFileCommandBase : ICommand
{
    /// <summary>
    /// Gets or sets the buffer to store values.
    /// </summary>
    [VariableValueBuffer]
    public IReadOnlyList<string>? Buffer { get; set; }

    /// <summary>
    /// Gets or sets whether to execute the operation regardless of the existence of existing files at the destination.
    /// </summary>
    [Option('f', false)]
    public bool Force { get; set; }

    /// <summary>
    /// Performs the operation.
    /// </summary>
    /// <param name="source">The file to operate.</param>
    /// <param name="destination">The calculated destination of the specified file.</param>
    public abstract void DoOperation(string source, string destination);

    /// <inheritdoc />
    public int Execute(IShell shell, ICommandExecutor executor, ExecutionContext context, out TextReader? pipeStream)
    {
        pipeStream = null;

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
