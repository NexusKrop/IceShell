namespace NexusKrop.IceShell.Core.Commands.Conversion;

public class IceSingleConverter : IceValueConverter
{
    public bool TryConvert(string str, out object? value)
    {
        var success = float.TryParse(str, out var x);
        value = x;
        return success;
    }
}