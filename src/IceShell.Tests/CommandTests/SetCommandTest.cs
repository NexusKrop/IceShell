namespace IceShell.Tests.CommandTests;

using IceShell.Core;
using IceShell.Core.Commands;
using IceShell.Core.Commands.Bundled;
using Moq;
using System;

public class SetCommandTest
{
    public const string TestVarName = "ICESHELL_DEV_TEST_SET_COMMAND";

    [Test]
    public void SetCommand_SetVar()
    {
        var set = new SetCommand
        {
            VariableName = TestVarName,
            VariableValue = "ICESHELL_DEV_TEST_VALUE"
        };

        Assert.Multiple(() =>
        {
            Assert.That(set.Execute(Mock.Of<IShell>(), Mock.Of<ICommandExecutor>(), ExecutionContext.Default).ExitCode, Is.EqualTo(0));
            Assert.That(Environment.GetEnvironmentVariable(TestVarName), Is.EqualTo("ICESHELL_DEV_TEST_VALUE"));
        });
    }

    [SetUp]
    public void SetUp()
    {
        Environment.SetEnvironmentVariable(TestVarName, null);
    }
}
