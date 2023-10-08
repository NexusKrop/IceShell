// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Bundled;

using global::IceShell.Core;
using global::IceShell.Core.Commands;
using global::IceShell.Core.Commands.Attributes;
using global::IceShell.Core.Exceptions;
using NexusKrop.IceShell.Core.Commands.Complex;
using NexusKrop.IceShell.Core.FileSystem;

/// <summary>
/// Defines a command that prints out the contents of a file.
/// </summary>
[ComplexCommand("type", "Displays the contents of a text file.")]
public class TypeCommandEx : ICommand
{
    /// <summary>
    /// The file to display.
    /// </summary>
    [Value("file", position: 0)]
    public string? ArgFile { get; set; }

    /// <summary>
    /// Whether to display the file in a streamed fashion.
    /// </summary>
    [Option('S', false)]
    public bool Streamed { get; set; }

    /// <inheritdoc />
    public int Execute(IShell shell, ICommandExecutor executor, ExecutionContext context, out TextReader? pipeStream)
    {
        if (string.IsNullOrWhiteSpace(ArgFile))
        {
            throw ExceptionHelper.ExceptedString();
        }

        var realFile = PathSearcher.ExpandVariables(ArgFile!);
        CommandChecks.FileExists(realFile);

        ExecuteStreamed(realFile, out pipeStream);

        return 0;
    }

    private static void ExecuteStreamed(string file, out TextReader pipeStream)
    {
        pipeStream = new StreamReader(file);
    }
}
