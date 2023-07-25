namespace NexusKrop.IceShell.Core.Commands.Conversion;

public class IceInt64Converter : IceValueConverter
{
    public bool TryConvert(string str, out object? value)
    {
        var success = long.TryParse(str, out var x);
        value = x;
        return success;
    }
}