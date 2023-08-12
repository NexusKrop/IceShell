// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Bundled;

using global::IceShell.Core;
using global::IceShell.Core.Commands.Attributes;
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
    [Value("target", position: 0)]
    public string? Target { get; set; }

    public int Execute(ComplexArgumentParseResult argument, IShell shell)
    {
        if (!File.Exists(Target))
        {
            throw new CommandFormatException(Messages.BadFileGeneric);
        }

        try
        {
            IceCube.Util.Shell.ShellExecute(Target);
        }
        catch (Win32Exception x) when (x.NativeErrorCode == 1155)
        {
            throw new CommandFormatException(Messages.BadFileHandler);
        }

        return 0;
    }
}
