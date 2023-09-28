// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Core.Api;

using IceShell.Core.Commands;

/// <summary>
/// Represents a command dispatcher.
/// </summary>
public interface ICommandDispatcher
{
    /// <summary>
    /// Gets the command manager for this instance.
    /// </summary>
    public ICommandManager CommandManager { get; }

    /// <summary>
    /// Executes the specified compound of command sections. If any of the sections in the compound returned
    /// non-zero value, the execution is interrupted and this method returns with the return value of such section.
    /// </summary>
    /// <param name="compound">The compound to execute.</param>
    /// <param name="executor">The executor to act on behalf of.</param>
    /// <returns>The return code of the commands. Usually, <c>0</c> means success.</returns>
    int Execute(CommandSectionCompound compound, ICommandExecutor executor);

    /// <summary>
    /// Executes the specified command section.
    /// </summary>
    /// <param name="section">The section to execute.</param>
    /// <param name="executor">The executor to act on behalf of.</param>
    /// <param name="outStream">The text reader to read the output of the command.</param>
    /// <param name="context">The execution context of this execution. Each context must be specific to execution of each section.</param>
    /// <returns>The return code of the section. Usually, <c>0</c> means success.</returns>
    int Execute(CommandSection section, ICommandExecutor executor, out TextReader? outStream, ExecutionContext? context = null);

    /// <summary>
    /// Executes the specified unit.
    /// </summary>
    /// <param name="command">The unit to execute.</param>
    /// <param name="executor">The executor to act on behalf of.</param>
    /// <param name="outStream">The text reader to read the output of the command.</param>
    /// <param name="context">The execution context of this execution.</param>
    /// <returns>The return code of the section. Usually, <c>0</c> means success.</returns>
    int Execute(CommandUnit command, ICommandExecutor executor, out TextReader? outStream, ExecutionContext context);
}
