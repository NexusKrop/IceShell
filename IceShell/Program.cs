// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

using IceShell.Batching;
using IceShell.Core.CLI.Languages;
using NexusKrop.IceCube.Settings;
using NexusKrop.IceShell.Core;
using NexusKrop.IceShell.Core.Api;
using Spectre.Console;

if (args.Length > 1)
{
    Console.WriteLine("Invalid arguments for IceShell launcher");
    Console.WriteLine("Usage: iceshell [command]");
    return 2;
}

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

Languages.Instance.Load();
Languages.SetInstanceConfig(sh);

var shell = new Shell(sh);

Shell.ModuleManager.AddModule(new BatchingModule());

if (args.Length == 0)
{
    return shell.RunInteractive();
}
else if (args.Length == 1)
{
    Shell.ModuleManager.InitializeModules();
    shell.Execute(args[0]);
}

return -255;
