// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

using NexusKrop.IceCube.Settings;
using NexusKrop.IceShell.Core;

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
else
{
    try
    {
        sh = new ShellSettings();
        sh.Save(configPath);
    }
    catch (Exception ex)
    {
        System.Console.WriteLine("Unable to save config file!");
        System.Console.WriteLine(ex);
        System.Console.WriteLine();
    }
}

sh ??= new();

return new Shell(sh).RunInteractive();
