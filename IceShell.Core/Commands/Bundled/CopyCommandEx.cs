namespace NexusKrop.IceShell.Core.Commands.Bundled;

using NexusKrop.IceShell.Core.Commands.Complex;
using NexusKrop.IceShell.Core.Exceptions;
using NexusKrop.IceShell.Core.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[ComplexCommand("copy", "Copies a file to another location.")]
public class CopyCommandEx : IComplexCommand
{
    //public void Define(ComplexArgument argument)
    //{
    //    argument.AddValue("source", true);
    //    argument.AddValue("destination", true);
    //    argument.AddOption('f', false);
    //}

    public int Execute(ComplexArgumentParseResult argument, Shell shell)
    {
        // TODO fix

        //var realSource = PathSearcher.ShellToSystem(argument.Values[0]!);
        //var realDest = PathSearcher.ShellToSystem(argument.Values[1]!);
        //var force = argument.OptionPresents('f');

        //CommandChecks.FileExists(realSource);
        //CommandChecks.DirectoryNotExists(realDest);

        //if (File.Exists(realDest) && !force)
        //{
        //    throw ExceptionHelper.WithName(Messages.MkdirFileExists, realDest);
        //}

        //try
        //{
        //    File.Copy(realSource, realDest, force);
        //}
        //catch (UnauthorizedAccessException)
        //{
        //    throw new CommandFormatException(Messages.FileUnauthorized);
        //}

        return 0;
    }
}
