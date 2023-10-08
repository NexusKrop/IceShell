namespace IceShell.Tests;

using IceShell.Core.Exceptions;
using NexusKrop.IceShell.Core.FileSystem;

public class PathSearcherTests
{
    [Test]
    [Platform(Include = "Win", Reason = "Windows path format specific test")]
    public void IsRooted_WindowsButUnixPath()
    {
        Assert.That(PathSearcher.IsRooted("/"), Is.False);
    }

    [Test]
    [Platform(Include = "Win", Reason = "Windows path format specific test")]
    public void IsRooted_WindowsButUnixPathChild()
    {
        Assert.That(PathSearcher.IsRooted("/home/test/"), Is.False);
    }

    [Test]
    [Platform(Include = "Win", Reason = "Windows path format specific test")]
    public void IsRooted_WindowsVolumeRoot()
    {
        Assert.That(PathSearcher.IsRooted(@"C:\"), Is.True);
    }

    [Test]
    [Platform(Include = "Win", Reason = "Windows path format specific test")]
    public void IsRooted_WindowsVolumeRootChild()
    {
        Assert.That(PathSearcher.IsRooted(@"C:\Windows\Test"), Is.True);
    }

    [Test]
    [Platform(Include = "Win", Reason = "Windows path format specific test")]
    public void IsRooted_WindowsButUnixWithWinSeparator()
    {
        Assert.That(PathSearcher.IsRooted("\\"), Is.False);
    }

    [Test]
    [Platform(Include = "Win", Reason = "Windows path format specific test")]
    public void IsRooted_WindowsButInternalPath()
    {
        Assert.That(PathSearcher.IsRooted("\\TestObject\\"), Is.False);
    }

    [Test]
    [Platform(Include = "Linux", Reason = "Linux path format specific test")]
    public void IsRooted_LinuxButWindowsPath()
    {
        Assert.That(PathSearcher.IsRooted(@"C:\"), Is.False);
    }

    [Test]
    [Platform(Include = "Linux", Reason = "Linux path format specific test")]
    public void IsRooted_LinuxButWindowsPathChild()
    {
        Assert.That(PathSearcher.IsRooted(@"C:\Windows\Test"), Is.False);
    }
    [Test]
    [Platform(Include = "Linux", Reason = "Linux path format specific test")]
    public void IsRooted_LinuxButWindowsPathWithForwardSlash()
    {
        Assert.That(PathSearcher.IsRooted(@"C:/"), Is.False);
    }


    [Test]
    [Platform(Include = "Linux", Reason = "Linux path format specific test")]
    public void IsRooted_LinuxButWindowsPathChildWithForwardSlash()
    {
        Assert.That(PathSearcher.IsRooted(@"C:/Windows/Test"), Is.False);
    }

    [Test]
    [Platform(Include = "Linux", Reason = "Linux path format specific test")]
    public void IsRooted_RootFileSystem()
    {
        Assert.That(PathSearcher.IsRooted("/"), Is.True);
    }

    [Test]
    [Platform(Include = "Linux", Reason = "Linux path format specific test")]
    public void IsRooted_RootFileSystemChild()
    {
        Assert.That(PathSearcher.IsRooted("/bin/sh"), Is.True);
    }

    [Test]
    public void ExpandPath_Variable()
    {
        var testVar = "ICESHELL_TEST_000_EXPANDPATH";
        var testName = "IceShell000Test";

        Environment.SetEnvironmentVariable(testName, testVar);

        var expanded = PathSearcher.ExpandVariables("%IceShell000Test%");

        Assert.That(expanded, Is.EqualTo(testVar));
    }

    [Test]
    public void ExpandPath_Escape()
    {
        Assert.That(PathSearcher.ExpandVariables("%%"),
            Is.EqualTo("%"));
    }

    [Test]
    public void ExpandPath_Complex()
    {
        var testVar = "ICESHELL_TEST_001_COMPLEX";
        var testName = "IceShell001Test";

        Environment.SetEnvironmentVariable(testName, testVar);

        Assert.That(PathSearcher.ExpandVariables("%IceShell001Test%%%"),
            Is.EqualTo("ICESHELL_TEST_001_COMPLEX%"));
    }

    [Test]
    public void ExpandPath_NonLenient_PathNeverEnds()
    {
        Assert.Throws<CommandFormatException>(() => PathSearcher.ExpandVariables("%"));
    }

    [Test]
    public void ExpandPath_NonLenient_NonExistingVariable()
    {
        Environment.SetEnvironmentVariable("IceShell002Test", null);

        Assert.Throws<CommandFormatException>(() => PathSearcher.ExpandVariables("%IceShell002Test%", false));
    }
}
