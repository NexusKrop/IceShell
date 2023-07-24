// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Bundled;

using NexusKrop.IceCube;
using NexusKrop.IceShell.Core.Commands.Complex;
using NexusKrop.IceShell.Core.Exceptions;
using System.ComponentModel;

/// <summary>
/// Defines a command that starts the specified file, either by executing it as as executable, or
/// opening it through an associated program.
/// </summary>
[ComplexCommand("start")]
public class StartCommandEx : IComplexCommand
{
    public void Define(ComplexArgument argument)
    {
        argument.AddValue(new("target", true));
    }

    public int Execute(ComplexArgumentParseResult argument)
    {
        var proc = argument.Values[0]!;

        if (!File.Exists(proc))
        {
            throw new CommandFormatException(Messages.BadFileGeneric);
        }

        try
        {
            IceCube.Util.Shell.ShellExecute(argument.Values[0]!);
        }
        catch (Win32Exception x) when (x.NativeErrorCode == 1155)
        {
            throw new CommandFormatException(Messages.BadFileHandler);
        }

        return 0;
    }
}
