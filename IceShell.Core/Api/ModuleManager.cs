// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Core.Api;

using IceShell.Core.CLI.Languages;
using NexusKrop.IceShell.Core.CLI;
using System;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// Provides module management interface and service for shell environments.
/// </summary>
public class ModuleManager
{
    private readonly List<IModule> _modules = new();

    internal void LoadModules(string directory)
    {
        if (!Directory.Exists(directory))
        {
            // Nothing to load
            return;
        }

        foreach (var file in Directory.GetFiles(directory, "*.dll"))
        {
            try
            {
                LoadModuleFrom(file);
            }
            catch (BadImageFormatException)
            {
                ConsoleOutput.PrintShellError($"Invalid plugin {file}");
            }
            catch (Exception ex)
            {
                ConsoleOutput.PrintShellError(ex.ToString());
#if !DEBUG
                throw;
#endif
            }
        }
    }

    /// <summary>
    /// Registers the specified module.
    /// </summary>
    /// <param name="module">The module to register.</param>
    public void AddModule(IModule module)
    {
        _modules.Add(module);
    }

    /// <summary>
    /// Attempts to create the instance of the specified module, and then registers the module.
    /// </summary>
    /// <param name="type">The type of the module to register.</param>
    /// <remarks>
    /// This method fails silently if the type specified is not a module.
    /// </remarks>
    public void AddModule(Type type)
    {
        var moduleI = type.GetInterface(nameof(IModule));

        if (moduleI != typeof(IModule) ||
            Activator.CreateInstance(type) is not IModule module)
        {
            return;
        }

        AddModule(module);
    }

    /// <summary>
    /// Calls the initialization function for all modules. This finalizes the process of module initialization process.
    /// </summary>
    public void InitializeModules(ICommandDispatcher dispatcher)
    {
        foreach (var module in _modules)
        {
            try
            {
                module.Initialize(dispatcher);
            }
            catch (Exception ex)
            {
                ConsoleOutput.WriteLineColour(string.Format(Languages.Get("init_module_fail"), module.GetType().Name), ConsoleColor.Red);
                ConsoleOutput.PrintShellError(ex.ToString());
            }
        }
    }

    /// <summary>
    /// Loads all modules in the specified assembly.
    /// </summary>
    /// <param name="assembly">The assembly.</param>
    public void LoadModuleFrom(Assembly assembly)
    {
        foreach (var type in assembly.GetTypes())
        {
            AddModule(type);
        }
    }

    /// <summary>
    /// Loads a module from the given path or file.
    /// </summary>
    /// <param name="file">The file.</param>
    /// <remarks>
    /// This method throws same exceptions as of <see cref="Assembly.LoadFrom(string)"/>.
    /// </remarks>
    public void LoadModuleFrom(string file)
    {
#pragma warning disable S3885
        // This was never intended to be anywhere near predictable
        // It is up to the user to determine whether the modules are safe
        var assembly = Assembly.LoadFrom(file);
#pragma warning restore S3885

        LoadModuleFrom(assembly);
    }
}
