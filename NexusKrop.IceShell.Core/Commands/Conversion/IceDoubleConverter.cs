namespace NexusKrop.IceShell.Core.Commands.Conversion;

public class IceDoubleConverter : IceValueConverter
{
    public bool TryConvert(string str, out object? value)
    {
        var success = double.TryParse(str, out var x);
        value = x;
        return success;
    }
}