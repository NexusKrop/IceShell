namespace IceShell.Tests;

using IceShell.Batching;
using IceShell.Core;
using IceShell.Core.Commands;
using Moq;

public class BatchTests
{
    [Test]
    public void BaseBatchTest()
    {
        var mock = new Mock<IShell>();

        var batch = BatchFile.Parse(new string[]
        {
            "baseline 1",
            "::baseline2",
            ":label",
            "baseline 3"
        });

        batch.RunBatch(mock.Object);

        mock.Verify(x => x.Execute("baseline 1", batch), Times.Once());
        mock.Verify(x => x.Execute("baseline 3", batch), Times.Once());
        mock.Verify(x => x.Execute("::baseline2", batch), Times.Never());
        mock.Verify(x => x.Execute(":label", batch), Times.Never());
    }

    [Test]
    public void BatchJumpTest()
    {
        var batch = BatchFile.Parse(new string[]
        {
            "baseline 1",
            "baseline 2",
            "goto label",
            "baseline 3",
            ":label",
            "baseline 4"
        });

        var mock = new Mock<IShell>();
        mock.Setup(x => x.Execute("goto label", batch))
            .Callback<string, ICommandExecutor>((str, ex) =>
            {
                batch.Jump(str.Split(' ')[1]);
            });

        batch.RunBatch(mock.Object);

        mock.Verify(x => x.Execute("baseline 1", batch), Times.Once());
        mock.Verify(x => x.Execute("baseline 2", batch), Times.Once());
        mock.Verify(x => x.Execute("baseline 4", batch), Times.Once());
        mock.Verify(x => x.Execute("baseline 3", batch), Times.Never());
    }
}
