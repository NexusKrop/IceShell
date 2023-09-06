// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Batching;

using IceShell.Batching.Commands;
using IceShell.Core.Api;
using NexusKrop.IceShell.Core;

public class BatchingModule : IModule
{
    public void Initialize()
    {
        Shell.CommandManager.RegisterComplex(typeof(GotoCommand));
        Shell.CommandManager.RegisterComplex(typeof(CallCommand));
    }
}
