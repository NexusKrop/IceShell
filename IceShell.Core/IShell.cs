// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Core;

using IceShell.Core.Commands;

/// <summary>
/// Represents a command-line shell interpreter.
/// </summary>
public interface IShell : ICommandExecutor
{
    /// <summary>
    /// Gets or sets the prompt of this instance.
    /// </summary>
    string Prompt { get; set; }

    /// <summary>
    /// Gets the command dispatcher for this instance.
    /// </summary>
    CommandDispatcher Dispatcher { get; }

    /// <summary>
    /// Instructs the active shell process to stop receiving new commands after the current one
    /// is complete.
    /// </summary>
    void Quit();

    /// <summary>
    /// Parses a command and execute it.
    /// </summary>
    /// <param name="line">The line to parse and execute.</param>
    /// <param name="actualExecutor">The executor to have this instance act on behalf of. If <see langword="null"/>, this instance will execute commands on its own behalf.</param>
    /// <returns>The return code of the command.</returns>
    int Execute(string line, ICommandExecutor? actualExecutor = null);

    /// <summary>
    /// Executes an already passed command compound.
    /// </summary>
    /// <param name="compound">The compound to execute.</param>
    /// <param name="actualExecutor">The executor to have this instance act on behalf of. If <see langword="null"/>, this instance will execute commands on its own behalf.</param>
    /// <returns>The return code of the command. Zero means success.</returns>
    int Execute(BatchLineCompound compound, ICommandExecutor? actualExecutor = null);
}
