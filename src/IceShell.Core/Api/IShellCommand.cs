// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Core.Api;

using IceShell.Core.Commands;

/// <summary>
/// Defines an IceShell internal command, with a specified command format.
/// </summary>
/// <seealso cref="ICommandManager"/>
/// <seealso cref="ICommandDispatcher"/>
public interface IShellCommand
{
    /// <summary>
    /// Executes this command.
    /// </summary>
    /// <param name="shell">The shell that backs the execution of this command.</param>
    /// <param name="executor">The executor that executes this command.</param>
    /// <param name="context">The execution context. It is not required that the context specified is unique for this execution.</param>
    /// <returns>The execution result.</returns>
    /// <exception cref="IceShell.Core.Exceptions.CommandInterruptException">The command was interrupted.</exception>
    /// <exception cref="IceShell.Core.Exceptions.CommandFormatException">There was a legacy format error.</exception>
    CommandResult Execute(IShell shell, ICommandExecutor executor, ExecutionContext context);
}
