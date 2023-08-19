namespace IceShell.Tests;

using IceShell.Core;
using IceShell.Core.Commands;
using IceShell.Core.Commands.Attributes;
using Moq;
using NexusKrop.IceShell.Core;
using NexusKrop.IceShell.Core.Commands.Complex;

public class CommandParseTest
{
    [VariableValue]
    [ComplexCommand("mocker", "Mock command")]
    internal class VarValues_MockCommand : ICommand
    {
        [VariableValueBuffer]
        public IReadOnlyList<string>? Buffer { get; set; }

        public int Execute(IShell shell, ICommandExecutor executor)
        {
            Assert.That(Buffer, Is.EqualTo(new string[] { "abc", "efg", "123" }));
            return 0;
        }
    }

    [Test]
    public void VarValuesTest()
    {
        var dispatcher = new CommandDispatcher(Mock.Of<IShell>());
        var parser = new CommandParser();
        parser.SetLine("abc efg 123");

        Shell.CommandManager.RegisterComplex(typeof(VarValues_MockCommand));

        Assert.DoesNotThrow(() => dispatcher.Execute(CommandDispatcher.Parse("mocker", parser),
            Mock.Of<ICommandExecutor>()));
    }
}