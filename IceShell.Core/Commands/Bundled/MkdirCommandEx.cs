namespace NexusKrop.IceShell.Core.Commands.Bundled;

using global::IceShell.Core;
using global::IceShell.Core.CLI.Languages;
using global::IceShell.Core.Commands.Complex;
using NexusKrop.IceShell.Core.Commands.Complex;
using NexusKrop.IceShell.Core.Exceptions;
using NexusKrop.IceShell.Core.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[ComplexCommand("mkdir", "Creates a directory.")]
[CommandAlias("md")]
public class MkdirCommandEx : IComplexCommand
{
    public void Define(ComplexArgument argument)
    {
        // TODO redefine
        //argument.AddValue(new("path", true));
        //argument.AddOption(new('i', false, false));
    }

    public int Execute(ComplexArgumentParseResult argument, IShell shell)
    {
        //var dir = PathSearcher.ShellToSystem(argument.Values[0]!);

        //if (File.Exists(dir))
        //{
        //    throw new CommandFormatException(Languages.MakeDirFileExists(dir));
        //}

        //if (Directory.Exists(dir) && !argument.Options.ContainsKey('i'))
        //{
        //    throw new CommandFormatException(Messages.MkdirDirectoryAlreadyExists);
        //}

        //try
        //{
        //    Directory.CreateDirectory(dir);
        //}
        //catch (UnauthorizedAccessException)
        //{
        //    throw new CommandFormatException(Messages.FileUnauthorizedCreate);
        //}

        return 0;
    }
}
