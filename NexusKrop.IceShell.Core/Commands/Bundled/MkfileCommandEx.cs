namespace NexusKrop.IceShell.Core.Commands.Bundled;

using NexusKrop.IceShell.Core.Commands.Complex;
using NexusKrop.IceShell.Core.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Defines a command that creates an empty file.
/// </summary>
[ComplexCommand("mkfile")]
public class MkfileCommandEx : IComplexCommand
{
    public void Define(ComplexArgument argument)
    {
        argument.AddValue("name", true);
    }

    public void Execute(ComplexArgumentParseResult argument)
    {
        var actual = PathSearcher.ShellToSystem(argument.Values[0]!);

        CommandChecks.FileNotExists(actual);

        // Create an absolutely empty file
        File.WriteAllBytes(actual, Array.Empty<byte>());
    }
}
