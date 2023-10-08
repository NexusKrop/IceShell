// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

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

    [Test]
    public void Parser_EmptyLine()
    {
        var parser = new LineParser();
        var retVal = parser.ParseLine(string.Empty);

        Assert.That(retVal, Is.Empty);
    }

    [Test]
    public void Parser_NoEscapeOutsideQuote()
    {
        var parser = new LineParser();
        var retVal = parser.ParseLine("Hey\\Hey");

        Assert.That(retVal[0], Is.EqualTo(new SyntaxStatement("Hey\\Hey", false)));
    }
}
