// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Bundled;

using global::IceShell.Core;
using global::IceShell.Core.CLI.Languages;
using global::IceShell.Core.Commands.Attributes;
using global::IceShell.Core.Commands.Bundled;
using Microsoft.Extensions.FileSystemGlobbing;
using NexusKrop.IceCube;
using NexusKrop.IceCube.Exceptions;
using NexusKrop.IceShell.Core.Commands.Complex;
using NexusKrop.IceShell.Core.FileSystem;

/// <summary>
/// Defines a command that moves or renames a file.
/// </summary>
[ComplexCommand("move", Description = "Move one or more files.")]
[VariableValue]
public class MoveCommandEx : DestinationFileCommandBase, ICommand
{
    /// <inheritdoc />
    public override void DoOperation(string source, string destination)
    {
        File.Move(source, destination);
    }
}
