// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Bundled;

using NexusKrop.IceShell.Core.Commands.Complex;
using System;
using System.Diagnostics;
using System.Reflection;

/// <summary>
/// Defines a command that shows the version of the operating system and the shell program.
/// </summary>
[ComplexCommand("ver", "Displays the IceShell version.")]
public class VerCommand : IComplexCommand
{
#if DEBUG
    private static readonly string PRODUCT_VERSION = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion ?? "unknown";
#else
    private static readonly string PRODUCT_VERSION = FileVersionInfo.GetVersionInfo(Environment.ProcessPath ?? "NexusKrop.IceShell.exe").ProductVersion ?? "unknown";
#endif

    public void Define(ComplexArgument argument)
    {
        // No arguments
    }

    public int Execute(ComplexArgumentParseResult argument)
    {
        Console.WriteLine(Messages.VerLine1, PRODUCT_VERSION);
        Console.WriteLine(Messages.VerLine2, Environment.OSVersion.VersionString);
        Console.WriteLine();
        Console.WriteLine(Messages.VerLine3);

        return 0;
    }
}
