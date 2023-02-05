﻿namespace NexusKrop.IceShell.Core.Api;

using NexusKrop.IceShell.Core.CLI;
using NexusKrop.IceShell.Core.Completion.Cache;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
    /// Loads a module from the given path or file.
    /// </summary>
    /// <param name="file">The file.</param>
    /// <remarks>
    /// This method throws same exceptions as of <see cref="Assembly.LoadFrom(string)(string)"/>.
    /// </remarks>
    public void LoadModuleFrom(string file)
    {
#pragma warning disable S3885
        // This was never intended to be anywhere near predictable
        // It is up to the user to determine whether the modules are safe
        var assembly = Assembly.LoadFrom(file);
#pragma warning restore S3885

        foreach (var type in assembly.GetTypes())
        {
            var moduleI = type.GetInterface(nameof(IModule));

            if (moduleI != typeof(IModule) ||
                Activator.CreateInstance(moduleI) is not IModule module)
            {
                continue;
            }

            _modules.Add(module);
        }
    }
}
