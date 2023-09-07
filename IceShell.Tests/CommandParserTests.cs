// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Tests;

using IceShell.Parsing;

public class CommandParserTests
{
    [Test]
    public void ParseCompound_Simple()
    {
        var statements = SyntaxStatement.Of("test", "argument", ">", "test1", "arg2");

        var parsed = CommandParser.ParseCompound(statements);

        Assert.Multiple(() =>
        {
            Assert.That(parsed.Segments, Has.Count.EqualTo(2));

            Assert.That(parsed.Segments[0].NextAction, Is.EqualTo(SyntaxNextAction.Redirect));
            Assert.That(parsed.Segments[0].Command?.Name, Is.EqualTo("test"));
            Assert.That(parsed.Segments[0].Command?.Options, Is.Empty);
            Assert.That(parsed.Segments[0].Command?.Values, Has.Count.EqualTo(1));
            Assert.That(parsed.Segments[0].Command?.Values[0].Content, Is.EqualTo("argument"));

            Assert.That(parsed.Segments[1].NextAction, Is.EqualTo(SyntaxNextAction.None));
            Assert.That(parsed.Segments[1].Command?.Name, Is.EqualTo("test1"));
            Assert.That(parsed.Segments[1].Command?.Options, Is.Empty);
            Assert.That(parsed.Segments[1].Command?.Values, Has.Count.EqualTo(1));
            Assert.That(parsed.Segments[1].Command?.Values[0].Content, Is.EqualTo("arg2"));
        });
    }

    [Test]
    public void ParseOption_NoValueOption()
    {
        var statement = new SyntaxStatement("/T", false);
        var retVal = CommandParser.ParseOption(statement, null, out _);

        Assert.That(retVal, Is.EqualTo(new SyntaxOption('T', null)));
    }

    [Test]
    public void ParseOption_SimpleOption()
    {
        var statement = new SyntaxStatement("/T:test_value", false);
        var retVal = CommandParser.ParseOption(statement, null, out _);

        Assert.That(retVal, Is.EqualTo(new SyntaxOption('T', "test_value")));
    }

    [Test]
    public void ParseOption_CompoundOption()
    {
        var statementFirst = new SyntaxStatement("/T:", false);
        var statementSecond = new SyntaxStatement("Test text", true);

        var retVal = CommandParser.ParseOption(statementFirst, statementSecond, out _);

        Assert.That(retVal, Is.EqualTo(new SyntaxOption('T', "Test text")));
    }

    [Test]
    public void ParseOption_OptionAndUnixPath()
    {
        var statements = new SyntaxStatement[]{
            new SyntaxStatement("command", false),
            new SyntaxStatement("/O", false),
            new SyntaxStatement("/bin/bash", false) };

        var retVal = CommandParser.ParseSingleCommand(statements);
        Assert.Multiple(() =>
        {
            Assert.That(retVal.Name, Is.EqualTo("command"));

            Assert.That(retVal.Values, Is.EqualTo(new HashSet<SyntaxStatement>()
            {
                new SyntaxStatement("/bin/bash", false)
            }));

            Assert.That(retVal.Options, Is.EqualTo(new HashSet<SyntaxOption>()
            {
                new SyntaxOption('O', null)
            }));
        });
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
