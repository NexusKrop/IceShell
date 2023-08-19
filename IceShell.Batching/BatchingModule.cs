namespace IceShell.Batching;

using IceShell.Batching.Commands;
using NexusKrop.IceShell.Core;
using NexusKrop.IceShell.Core.Api;

public class BatchingModule : IModule
{
    public void Initialize()
    {
        Shell.CommandManager.RegisterComplex(typeof(GotoCommand));
        Shell.CommandManager.RegisterComplex(typeof(CallCommand));
    }
}
