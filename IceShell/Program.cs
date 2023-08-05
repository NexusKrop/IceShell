// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

using NexusKrop.IceCube.Settings;
using NexusKrop.IceShell.Core;
using Spectre.Console;

var userPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".iceShell");
Directory.CreateDirectory(userPath);

var configPath = Path.Combine(userPath, "config.ini");

ShellSettings? sh = null;

// Initialize configuration
if (File.Exists(configPath))
{
    try
    {
        sh = ShellSettings.LoadFromFile(configPath);
    }
    catch (Exception ex)
    {
        System.Console.WriteLine("Unable to load config file!");
        System.Console.WriteLine(ex);
        System.Console.WriteLine();
    }
}

// Create if possible
if (sh == null)
{
    try
    {
        sh = new ShellSettings();
        sh.Save(configPath);
    }
    catch (Exception ex)
    {
        AnsiConsole.WriteLine("[red]Unable to create configuration![/]");
        AnsiConsole.WriteException(ex);
        sh = new();
    }
}

return new Shell(sh).RunInteractive();
