namespace IceShell.Tests;

using IceShell.Core;
using System;

public class ExecutiveTests
{
    [Test]
    public void TryParsePrompt_CurrentDate()
    {
        Assert.Multiple(() =>
        {
            Assert.That(Executive.TryParsePrompt("$D", out var result), Is.True);
            Assert.That(result,
                Is.EqualTo(DateTime.Now.ToShortDateString()));
        });
    }

    [Test]
    public void TryParsePrompt_CurrentTime()
    {
        Assert.Multiple(() =>
        {
            Assert.That(Executive.TryParsePrompt("$T", out var result), Is.True);
            Assert.That(result,
                Is.EqualTo(DateTime.Now.ToShortTimeString()));
        });
    }

    [Test]
    public void TryParsePrompt_CurrentDirectory()
    {
        Assert.Multiple(() =>
        {
            Assert.That(Executive.TryParsePrompt("$P", out var result), Is.True);
            Assert.That(result,
                Is.EqualTo(Directory.GetCurrentDirectory()));
        });
    }

    [Test]
    public void TryParsePrompt_CurrentDirectoryAndGreaterThan()
    {
        Assert.Multiple(() =>
        {
            Assert.That(Executive.TryParsePrompt("$P$G", out var result), Is.True);
            Assert.That(result,
                Is.EqualTo($"{Directory.GetCurrentDirectory()}>"));
        });
    }

    [Test]
    public void TryParsePrompt_CurrentDirectoryAndGreaterThanWithColon()
    {
        Assert.Multiple(() =>
        {
            Assert.That(Executive.TryParsePrompt("$P$G:", out var result), Is.True);
            Assert.That(result,
                Is.EqualTo($"{Directory.GetCurrentDirectory()}>:"));
        });
    }

    [Test]
    public void TryParsePrompt_CurrentDirectoryWithRandomSymbols()
    {
        Assert.Multiple(() =>
        {
            Assert.That(Executive.TryParsePrompt("$P$G:(())", out var result), Is.True);
            Assert.That(result,
                Is.EqualTo($"{Directory.GetCurrentDirectory()}>:(())"));
        });
    }

    [Test]
    public void TryParsePrompt_Fail_PlaceHolderNeverEnds()
    {
        Assert.That(Executive.TryParsePrompt("$", out _), Is.False);
    }

    [Test]
    public void TryParsePrompt_Fail_InvalidPlaceholder()
    {
        Assert.That(Executive.TryParsePrompt("$%", out _), Is.False);
    }
}
