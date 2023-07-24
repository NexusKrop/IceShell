// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Complex;

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
public interface IComplexCommand
{
    /// <summary>
    /// Defines the arguments of this command. This method is called every time before execution.
    /// </summary>
    /// <param name="argument">The argument parsing service.</param>
    void Define(ComplexArgument argument);

    /// <summary>
    /// Executes this command.
    /// </summary>
    /// <param name="argument">The parsed argument.</param>
    /// <returns>The execution result code. If <c>0</c>, the command is considered success.</returns>
    int Execute(ComplexArgumentParseResult argument);
}
