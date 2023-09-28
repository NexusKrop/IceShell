// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Core.Api;

using IceShell.Core.Commands;

/// <summary>
/// Defines a module implementation.
/// </summary>
public interface IModule
{
    /// <summary>
    /// Called when initializing the module.
    /// </summary>
    void Initialize(ICommandDispatcher dispatcher);
}
