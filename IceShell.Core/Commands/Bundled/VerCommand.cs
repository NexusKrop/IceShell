// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Bundled;

using global::IceShell.Core;
using global::IceShell.Core.CLI.Languages;
using global::IceShell.Core.Commands;
using NexusKrop.IceShell.Core.Commands.Complex;
using System;
using System.Diagnostics;
using System.Reflection;

/// <summary>
/// Defines a command that shows the version of the operating system and the shell program.
/// </summary>
[ComplexCommand("ver", "Displays the IceShell version.")]
public class VerCommand : ICommand
{
#if DEBUG
    private static readonly string PRODUCT_VERSION = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion ?? "unknown";
#else
    private static readonly string PRODUCT_VERSION = FileVersionInfo.GetVersionInfo(Environment.ProcessPath ?? "NexusKrop.IceShell.exe").ProductVersion ?? "unknown";
#endif

    public int Execute(IShell shell, ICommandExecutor executor)
    {
        Console.WriteLine(Languages.Get("ver_line_0"), PRODUCT_VERSION);
        Console.WriteLine(Languages.Get("ver_line_1"), Environment.OSVersion.VersionString);
        Console.WriteLine();
        Console.WriteLine(Languages.Get("ver_line_2"));

        return 0;
    }
}
