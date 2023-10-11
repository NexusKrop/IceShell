namespace IceShell.Tests.Converters;

using IceShell.Core.Commands.Argument;
using IceShell.Core.Exceptions;

public class EnumerationConverterTests
{
    public FileAccess DumbProperty { get; set; } = FileAccess.Read;

    [SetUp]
    public void SetUp()
    {
        DumbProperty = FileAccess.Read;
    }

    [Test]
    public void Convert_InvalidValue()
    {
        var converter = new EnumerationConverter();

        Assert.Throws<CommandFormatException>(() =>
        {
            converter.Convert("some illegal value", GetType().GetProperty(nameof(DumbProperty))!, this);
        });
    }

    [Test]
    public void Convert_ValidValue()
    {
        var converter = new EnumerationConverter();

        Assert.DoesNotThrow(() =>
        {
            converter.Convert("Write", GetType().GetProperty(nameof(DumbProperty))!, this);
        });

        Assert.That(DumbProperty, Is.EqualTo(FileAccess.Write));
    }
}
