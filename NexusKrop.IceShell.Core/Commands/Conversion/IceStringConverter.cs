namespace NexusKrop.IceShell.Core.Commands.Conversion;

public class IceStringConverter : IceValueConverter
{
    public bool TryConvert(string str, out object? value)
    {
        // Convert to a string, no need special attention.
        value = str;
        return true;
    }
}