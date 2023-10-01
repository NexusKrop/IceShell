namespace IceShell.Tests;

using IceShell.Settings;
using IniParser.Model;

public class ShellSettingsTest
{
    [Test]
    public void LoadFrom_EmptyNoException()
    {
        var iniData = new IniData();

        Assert.DoesNotThrow(() => ShellSettings.LoadFrom(iniData));
    }
}
