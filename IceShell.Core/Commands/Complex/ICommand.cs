// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Complex;

using global::IceShell.Core;
using global::IceShell.Core.Commands;

/// <summary>
/// Defines a complex command, with variable amount of arguments, and with support of option and value arguments.
/// </summary>
/// <remarks>
/// <para>
/// The reason of why complex commands are called complex commands is because there were
/// a type of command, in the early stage of development of IceShell, called just <c>Command</c>, which uses a fixed number of arguments, without any support of options, only values.
/// Complex commands, which uses a variable amount of arguments, was introduced later, co-existing with the <c>Command</c>s. Later when introducing auto completion, the simple commands (the Commands) were scrapped and all commands that
/// were simple commands are ported to use complex command system.
/// </para>
/// </remarks>
/// <seealso cref="CommandManager"/>
/// <seealso cref="ComplexCommandAttribute"/>
public interface ICommand
{
    /// <summary>
    /// Executes this command.
    /// </summary>
    /// <param name="shell">The shell that this command was executed with.</param>
    /// <param name="executor">The executor that this command was executed by.</param>
    /// <param name="context">The execution context. The instance provided may not be unique for this execution event.</param>
    /// <returns>The execution result code. If <c>0</c>, the command is considered success.</returns>
    int Execute(IShell shell, ICommandExecutor executor, ExecutionContext context);
}
