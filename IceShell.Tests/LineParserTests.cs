namespace IceShell.Tests;

using IceShell.Parsing;

public class LineParserTests
{
    [Test]
    public void BasicParseTest()
    {
        var parser = new LineParser();
        var retVal = parser.ParseLine("command /F /V:\"value in prop\" /Q:varr normal_value \"quoted value\"");

        Assert.That(retVal, Is.EqualTo(new SyntaxStatement[]
        {
            new("command", false),
            new("/F", false),
            new("/V:", false),
            new("value in prop", true),
            new("/Q:varr", false),
            new("normal_value", false),
            new("quoted value", true)
        }));
    }
}
