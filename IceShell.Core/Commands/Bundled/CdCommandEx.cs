// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Bundled;

using global::IceShell.Core;
using global::IceShell.Core.Commands;
using global::IceShell.Core.Commands.Attributes;
using global::IceShell.Core.Commands.Complex;
using global::IceShell.Core.Exceptions;
using NexusKrop.IceShell.Core.Commands.Complex;
using NexusKrop.IceShell.Core.FileSystem;

/// <summary>
/// Displays the name of or changes the current directory.
/// </summary>
[ComplexCommand("cd", "Displays the name of or changes the current directory.")]
[CommandAlias("chdir")]
[GreedyString]
public class CdCommandEx : ICommand
{
    /// <summary>
    /// Gets or sets the directory to change the current directory to.
    /// </summary>
    /// <value>
    /// The directory to change the current directory to. If <see langword="null"/>, displays the name of the
    /// current directory.
    /// </value>
    [Value("destination", position: 0)]
    public string? Destination { get; set; }

    /// <inheritdoc />
    public int Execute(IShell shell, ICommandExecutor executor, ExecutionContext context, out TextReader? pipeStream)
    {
        pipeStream = null;

        if (string.IsNullOrWhiteSpace(Destination))
        {
            // Print current directory if no current directory is provided
            Console.WriteLine(Environment.CurrentDirectory);
            return 0;
        }

        var target = Destination ?? "";

        if (!Directory.Exists(target))
        {
            throw ExceptionHelper.BadDirectory(target);
        }

        Shell.ChangeDirectory(target);

        return 0;
    }
}
