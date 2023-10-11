namespace IceShell.Platform.Windows;

using IceShell.Core.Api;

public class WindowsPlatformModule : IModule
{
    public void Initialize(ICommandDispatcher dispatcher)
    {
        dispatcher.CommandManager.Register(typeof(VolumeInfoCommand));
    }
}
