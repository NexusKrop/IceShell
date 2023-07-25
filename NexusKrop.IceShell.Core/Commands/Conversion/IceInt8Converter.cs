namespace NexusKrop.IceShell.Core.Commands.Conversion;

public class IceInt8Converter : IceValueConverter
{
    public bool TryConvert(string str, out object? value)
    {
        var success = byte.TryParse(str, out var x);
        value = x;
        return success;
    }
}