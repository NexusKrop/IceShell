namespace NexusKrop.IceShell.Core.Commands.Bundled;

using global::IceShell.Core;
using global::IceShell.Core.CLI.Languages;
using global::IceShell.Core.Commands;
using global::IceShell.Core.Commands.Attributes;
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
public class MkdirCommandEx : ICommand
{
    [Value("directory", true, 0)]
    public string? DirectoryName { get; set; }

    public int Execute(IShell shell, ICommandExecutor executor)
    {
        var dir = PathSearcher.ShellToSystem(DirectoryName);

        CommandChecks.NothingExists(dir);

        try
        {
            Directory.CreateDirectory(dir);
        }
        catch (UnauthorizedAccessException)
        {
            throw ExceptionHelper.UnauthorizedWrite();
        }

        return 0;
    }
}
