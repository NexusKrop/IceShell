namespace IceShell.Tests;

using IceShell.Parsing;

public class CommandParserTests
{
    [Test]
    public void ParseOption_NoValueOption()
    {
        var statement = new SyntaxStatement("/T", false);
        var retVal = CommandParser.ParseOption(statement, null);

        Assert.That(retVal, Is.EqualTo(new SyntaxOption('T', null)));
    }

    [Test]
    public void ParseOption_SimpleOption()
    {
        var statement = new SyntaxStatement("/T:test_value", false);
        var retVal = CommandParser.ParseOption(statement, null);

        Assert.That(retVal, Is.EqualTo(new SyntaxOption('T', "test_value")));
    }

    [Test]
    public void ParseOption_CompoundOption()
    {
        var statementFirst = new SyntaxStatement("/T:", false);
        var statementSecond = new SyntaxStatement("Test text", true);

        var retVal = CommandParser.ParseOption(statementFirst, statementSecond);

        Assert.That(retVal, Is.EqualTo(new SyntaxOption('T', "Test text")));
    }

    [Test]
    public void ParseCommand_Empty()
    {
        var retVal = CommandParser.ParseSingleCommand(Array.Empty<SyntaxStatement>());
        Assert.Multiple(() =>
        {
            Assert.That(retVal.Name, Is.EqualTo(string.Empty));
            Assert.That(retVal.Options, Is.Empty);
            Assert.That(retVal.Values, Is.Empty);
        });
    }

    [Test]
    public void ParseCommand_OptionsOnly()
    {
        var statements = new LineParser().ParseLine("command /T:test_value /Q:\"test value\" /X");
        var retVal = CommandParser.ParseSingleCommand(statements);

        Assert.Multiple(() =>
        {
            Assert.That(retVal.Name, Is.EqualTo("command"));
            Assert.That(retVal.Options, Is.EqualTo(new HashSet<SyntaxOption>()
            {
                new SyntaxOption('T', "test_value"),
                new SyntaxOption('Q', "test value"),
                new SyntaxOption('X', null)
            }));
            Assert.That(!retVal.Values.Any());
        });
    }

    [Test]
    public void ParseCommand_ValuesOnly()
    {
        var statements = new LineParser().ParseLine("command \"quoted value\" simple_value");
        var retVal = CommandParser.ParseSingleCommand(statements);

        Assert.Multiple(() =>
        {
            Assert.That(retVal.Name, Is.EqualTo("command"));
            Assert.That(retVal.Values, Is.EqualTo(new HashSet<SyntaxStatement>()
            {
                new SyntaxStatement("quoted value", true),
                new SyntaxStatement("simple_value", false)
            }));
            Assert.That(!retVal.Options.Any());
        });
    }

    [Test]
    public void ParseCommand_QuotedOptions()
    {
        var statements = new LineParser().ParseLine("command \"/X\" /M");
        var retVal = CommandParser.ParseSingleCommand(statements);

        Assert.Multiple(() =>
        {
            Assert.That(retVal.Name, Is.EqualTo("command"));
            Assert.That(retVal.Values, Is.EqualTo(new HashSet<SyntaxStatement>()
            {
                new SyntaxStatement("/X", true)
            }));
            Assert.That(retVal.Options, Is.EqualTo(new HashSet<SyntaxOption>()
            {
                new SyntaxOption('M', null)
            }));
        });
    }

    [Test]
    public void ParseCommand_EndOfOptions()
    {
        var statements = new LineParser().ParseLine("command /A /B -- /C");
        var retVal = CommandParser.ParseSingleCommand(statements);

        Assert.Multiple(() =>
        {
            Assert.That(retVal.Name, Is.EqualTo("command"));
            Assert.That(retVal.Options, Is.EqualTo(new HashSet<SyntaxOption>()
            {
                new SyntaxOption('A', null),
                new SyntaxOption('B', null)
            }));
            Assert.That(retVal.Values, Is.EqualTo(new HashSet<SyntaxStatement>()
            {
                new SyntaxStatement("/C", false)
            }));
        });
    }
}
