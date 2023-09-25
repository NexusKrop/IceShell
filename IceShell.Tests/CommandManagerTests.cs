namespace IceShell.Tests;

using NexusKrop.IceShell.Core.Commands;
using NexusKrop.IceShell.Core.Commands.Bundled;

public class CommandManagerTests
{
    /// <summary>
    /// A test that assures the behaviour of the <see cref="CommandManager.HasDefinition(string)"/> method.
    /// </summary>
    [Test]
    public void HasDefinition_BasicFeature()
    {
        var manager = new CommandManager(false);

        manager.RegisterComplex(typeof(EchoCommandEx));
        Assert.Multiple(() =>
        {
            Assert.That(manager.HasDefinition("echo"), Is.True);
            Assert.That(manager.HasDefinition("eChO"), Is.True);
            Assert.That(manager.HasDefinition("ECHO"), Is.True);
        });
    }
}
