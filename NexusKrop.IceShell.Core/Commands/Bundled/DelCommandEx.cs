namespace NexusKrop.IceShell.Core.Commands.Bundled;

using NexusKrop.IceCube.Exceptions;
using NexusKrop.IceShell.Core.Commands.Complex;
using NexusKrop.IceShell.Core.FileSystem;

/// <summary>
/// Defines a command that deletes a file.
/// </summary>
[ComplexCommand("del")]
public class DelCommandEx : IComplexCommand
{
    public void Define(ComplexArgument argument)
    {
        argument.AddValue("target", true);
    }

    private static void DeleteFileCommit(string file)
    {
        File.Delete(file);
    }

    public void Execute(ComplexArgumentParseResult argument)
    {
        var target = PathSearcher.ShellToSystem(argument.Values[0]!);

        Checks.FileExists(target);

        // Reserved for future use
        DeleteFileCommit(target);
    }
}
