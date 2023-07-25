namespace NexusKrop.IceShell.Core.Commands.Conversion;

public interface IceValueConverter
{
    bool TryConvert(string str, out object? value);
}
