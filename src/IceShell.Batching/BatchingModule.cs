// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Batching;

using IceShell.Batching.Commands;
using IceShell.Core.Api;

public class BatchingModule : IModule
{
    public void Initialize(ICommandDispatcher dispatcher)
    {
        dispatcher.CommandManager.Register(typeof(GotoCommand));
        dispatcher.CommandManager.Register(typeof(CallCommand));
    }
}
