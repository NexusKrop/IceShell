namespace IceShell.Tests;

using IceShell.Core;
using IceShell.Core.Commands;
using IceShell.Core.Commands.Attributes;
using NexusKrop.IceShell.Core;
using NexusKrop.IceShell.Core.Commands;
using NexusKrop.IceShell.Core.Commands.Complex;
using NSubstitute;

public class CommandParseTest
{
    [VariableValue]
    [ComplexCommand("mocker", "Mock command")]
    internal class VarValues_MockCommand : IComplexCommand
    {
        [VariableValueBuffer]
        public IReadOnlyList<string>? Buffer { get; set; }

        public int Execute(ComplexArgumentParseResult argument, IShell shell)
        {
            Assert.That(Buffer, Is.EqualTo(new string[] { "abc", "efg", "123" }));
            return 0;
        }
    }

    [Test]
    public void VarValuesTest()
    {
        var dispatcher = new CommandDispatcher(Substitute.For<IShell>());
        var parser = new CommandParser();
        parser.SetLine("abc efg 123");

        Shell.CommandManager.RegisterComplex(typeof(VarValues_MockCommand));

        Assert.DoesNotThrow(() => dispatcher.Execute(CommandDispatcher.Parse("mocker", parser)));
    }
}