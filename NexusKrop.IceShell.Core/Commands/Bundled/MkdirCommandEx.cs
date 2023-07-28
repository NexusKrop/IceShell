namespace NexusKrop.IceShell.Core.Commands.Bundled;

using NexusKrop.IceShell.Core.Commands.Complex;
using NexusKrop.IceShell.Core.Exceptions;
using NexusKrop.IceShell.Core.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[ComplexCommand("mkdir", "Creates a directory.")]
public class MkdirCommandEx : IComplexCommand
{
    public void Define(ComplexArgument argument)
    {
        argument.AddValue(new("path", true));
        argument.AddOption(new('i', false, false));
    }

    public int Execute(ComplexArgumentParseResult argument)
    {
        var dir = PathSearcher.ShellToSystem(argument.Values[0]!);

        if (File.Exists(dir))
        {
            throw new CommandFormatException(Messages.MkdirFileExists);
        }

        if (Directory.Exists(dir) && !argument.Options.ContainsKey('i'))
        {
            throw new CommandFormatException(Messages.MkdirDirectoryAlreadyExists);
        }

        try
        {
            Directory.CreateDirectory(dir);
        }
        catch (UnauthorizedAccessException)
        {
            throw new CommandFormatException(Messages.FileUnauthorizedCreate);
        }

        return 0;
    }
}
