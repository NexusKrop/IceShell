namespace IceShell.Tests;

using IceShell.Core;
using IceShell.Core.CLI.Languages;
using IceShell.Core.Commands;
using IceShell.Core.Commands.Attributes;
using Moq;
using NexusKrop.IceShell.Core;
using NexusKrop.IceShell.Core.Commands.Complex;

public class CommandParseTest
{
    [SetUp]
    public void Setup()
    {
        Languages.Instance.Reload();
    }

    [VariableValue]
    [ComplexCommand("vault", "Mock command")]
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

    [ComplexCommand("greed", "Mock command")]
    [GreedyString]
    internal class Greedy_MockCommand : ICommand
    {
        [Value("string", true, 0)]
        public string? String { get; set; }

        [Value("greedy", true, 1)]
        public string? Greedy { get; set; }

        public int Execute(IShell shell, ICommandExecutor executor)
        {
            Assert.That(String, Is.EqualTo("abc"));
            Assert.That(Greedy, Is.EqualTo("efg triathlon i do not know!!!"));
            return 0;
        }
    }

    [Test]
    public void GreedyValue_LastIsGreedy()
    {
        var dispatcher = new CommandDispatcher(Mock.Of<IShell>());
        var parser = new CommandParser();
        parser.SetLine("abc efg triathlon i do not know!!!");

        Shell.CommandManager.RegisterComplex(typeof(Greedy_MockCommand));

        Assert.That(dispatcher.Execute(CommandDispatcher.Parse("greed", parser),
            Mock.Of<ICommandExecutor>()), Is.EqualTo(0));
    }

    [Test]
    public void VarValuesTest()
    {
        var dispatcher = new CommandDispatcher(Mock.Of<IShell>());
        var parser = new CommandParser();
        parser.SetLine("abc efg 123");

        Shell.CommandManager.RegisterComplex(typeof(VarValues_MockCommand));

        Assert.DoesNotThrow(() => dispatcher.Execute(CommandDispatcher.Parse("vault", parser),
            Mock.Of<ICommandExecutor>()));
    }
}