namespace NexusKrop.IceShell.Core.Commands.Conversion;

public class IceInt32Converter : IceValueConverter
{
    public bool TryConvert(string str, out object? value)
    {
        var success = int.TryParse(str, out var x);
        value = x;
        return success;
    }
}