// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Core.Api;

using IceShell.Core.CLI.Languages;
using NexusKrop.IceShell.Core.CLI;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// Provides module management interface and service for shell environments.
/// </summary>
public class ModuleManager : IModuleManager
{
    private readonly List<IModule> _modules = new();
    private readonly ICommandDispatcher _commandDispatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="ModuleManager"/> class.
    /// </summary>
    /// <param name="commandDispatcher">The dispatcher.</param>
    public ModuleManager(ICommandDispatcher commandDispatcher)
    {
        _commandDispatcher = commandDispatcher;
    }

    /// <inheritdoc />
    public void LoadDirectory(string directory)
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
                LoadAssembly(file);
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

    /// <inheritdoc />
    public void AddModule(IModule module)
    {
        _modules.Add(module);
    }

    /// <inheritdoc />
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
    [Obsolete("Use Initialize instead.")]
    public void InitializeModules(ICommandDispatcher dispatcher)
    {
        Initialize();
    }

    /// <summary>
    /// Loads all modules in the specified assembly.
    /// </summary>
    /// <param name="assembly">The assembly.</param>
    [Obsolete("Use LoadAssembly(Assembly) instead.")]
    public void LoadModuleFrom(Assembly assembly)
    {
        LoadAssembly(assembly);
    }

    /// <summary>
    /// Loads a module from the given path or file.
    /// </summary>
    /// <param name="file">The file.</param>
    /// <remarks>
    /// This method throws same exceptions as of <see cref="Assembly.LoadFrom(string)"/>.
    /// </remarks>
    [Obsolete("Use LoadAssembly(string) instead.")]
    public void LoadModuleFrom(string file)
    {
        LoadAssembly(file);
    }

    /// <inheritdoc/>
    public void LoadAssembly(string file)
    {
#pragma warning disable S3885
        // This was never intended to be anywhere near predictable
        // It is up to the user to determine whether the modules are safe
        var assembly = Assembly.LoadFrom(file);
#pragma warning restore S3885

        LoadAssembly(assembly);
    }

    /// <inheritdoc/>
    public void LoadAssembly(Assembly assembly)
    {
        foreach (var type in assembly.GetTypes())
        {
            AddModule(type);
        }
    }

    /// <inheritdoc/>
    public void Initialize()
    {
        foreach (var module in _modules)
        {
            try
            {
                module.Initialize(_commandDispatcher);
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLineInterpolated($"[red]{string.Format(Languages.Get("init_module_fail"), module.GetType().Name)}[/]");
                ConsoleOutput.PrintShellError(ex.ToString());
            }
        }
    }
}
