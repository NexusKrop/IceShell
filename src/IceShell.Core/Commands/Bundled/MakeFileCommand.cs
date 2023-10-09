// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Bundled;

using global::IceShell.Core;
using global::IceShell.Core.CLI.Languages;
using global::IceShell.Core.Commands;
using global::IceShell.Core.Commands.Attributes;
using global::IceShell.Core.Exceptions;
using NexusKrop.IceCube.Exceptions;
using NexusKrop.IceShell.Core.Commands.Complex;
using NexusKrop.IceShell.Core.FileSystem;
using System;

/// <summary>
/// Defines a command that creates an empty file.
/// </summary>
[ComplexCommand("mkfile")]
public class MakeFileCommand : ICommand
{
    /// <summary>
    /// Gets or sets the name of the file to create.
    /// </summary>
    [Value("value", position: 0)]
    public string? Name { get; set; }

    /// <inheritdoc/>
    public int Execute(IShell shell, ICommandExecutor executor, ExecutionContext context, out TextReader? pipeStream)
    {
        pipeStream = null;
        var actual = PathSearcher.ExpandVariables(Name);

        Checks.ArgNotNull(actual);
        CommandChecks.FileNotExists(actual!);
        CommandChecks.DirectoryNotExists(actual!);

        try
        {
            // Create an absolutely empty file
            File.WriteAllBytes(actual, Array.Empty<byte>());
        }
        catch (UnauthorizedAccessException)
        {
            throw ExceptionHelper.UnauthorizedWrite();
        }

        return 0;
    }
}