namespace NexusKrop.IceShell.Core.Commands.Bundled;

using global::IceShell.Core;
using global::IceShell.Core.CLI.Languages;
using global::IceShell.Core.Commands.Attributes;
using Microsoft.Extensions.FileSystemGlobbing;
using NexusKrop.IceCube;
using NexusKrop.IceShell.Core.Commands.Complex;
using NexusKrop.IceShell.Core.Exceptions;
using NexusKrop.IceShell.Core.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[ComplexCommand("copy", "Copies a file to another location.", CustomUsage = "<sources...> <destination>")]
[VariableValue]
public class CopyCommandEx : IComplexCommand
{
    [VariableValueBuffer]
    public IReadOnlyList<string>? Buffer { get; set; }

    [Option('f', false)]
    public bool Force { get; set; }

    public int Execute(ComplexArgumentParseResult argument, IShell shell)
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
                destination = PathSearcher.ShellToSystem(str);
                continue;
            }

            matcher.AddInclude(str);
        }

        var destIsDir = Directory.Exists(destination) || destination.EndsWith(Path.DirectorySeparatorChar);

        var realFiles = matcher.GetResultsInFullPath(Directory.GetCurrentDirectory());
        var toCopy = new Dictionary<string, string>();

        foreach (var file in realFiles)
        {
            if (destIsDir)
            {
                toCopy.Add(file, Path.Combine(destination, Path.GetFileName(file)));
            }
            else
            {
                toCopy.Add(file, destination);
            }
        }

        if (!Directory.Exists(destination) && toCopy.Count > 2)
        {
            throw new CommandFormatException(Languages.CopyIsFileMore());
        }

        toCopy.ForEach(x => File.Copy(x.Key, x.Value));

        return 0;
    }
}
