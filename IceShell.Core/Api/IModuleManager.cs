// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Core.Api;
using System;
using System.Reflection;

/// <summary>
/// Defines an interface for module management.
/// </summary>
public interface IModuleManager
{
    /// <summary>
    /// Adds an instance of <see cref="IModule"/> to the module registration.
    /// </summary>
    /// <param name="module">The module to add.</param>
    void AddModule(IModule module);

    /// <summary>
    /// Adds a <see cref="Type"/> representing a module to the module registration.
    /// </summary>
    /// <param name="type">The type to add.</param>
    void AddModule(Type type);

    /// <summary>
    /// Loads all modules from the specified assembly file.
    /// </summary>
    /// <param name="file">The assembly file to load modules from.</param>
    void LoadAssembly(string file);

    /// <summary>
    /// Loads all modules from the specified assembly.
    /// </summary>
    /// <param name="assembly">The assembly to load modules from.</param>
    void LoadAssembly(Assembly assembly);

    /// <summary>
    /// Searches for modules in the specified directory, and load the searched modules.
    /// </summary>
    /// <param name="directory">The directory to load modules from.</param>
    void LoadDirectory(string directory);

    /// <summary>
    /// Initializes all modules in module registrations.
    /// </summary>
    void Initialize();
}
