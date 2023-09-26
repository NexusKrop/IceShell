// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Bundled;

using global::IceShell.Core;
using global::IceShell.Core.CLI.Languages;
using global::IceShell.Core.Commands;
using NexusKrop.IceShell.Core.Commands.Complex;
using System;

/// <summary>
/// Defines a command that shows the version of the operating system and the shell program.
/// </summary>
[ComplexCommand("ver", "Displays the IceShell version.")]
public class ShellVersionCommand : ICommand
{
    /// <inheritdoc />
    public int Execute(IShell shell, ICommandExecutor executor, ExecutionContext context, out TextReader? pipeStream)
    {
        Console.WriteLine(Languages.Get("ver_line_0"), Shell.AppVersion);
        Console.WriteLine(Languages.Get("ver_line_1"), Environment.OSVersion.VersionString);
        Console.WriteLine();
        Console.WriteLine(Languages.Get("ver_line_2"));

        pipeStream = null;
        return 0;
    }
}
