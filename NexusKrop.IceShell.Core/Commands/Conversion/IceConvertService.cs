namespace NexusKrop.IceShell.Core.Commands.Conversion;

public static class IceConvertService
{
    private static readonly Dictionary<Type, IceValueConverter> Converters = new()
    {
        { typeof(string), new IceStringConverter() },
        { typeof(byte), new IceInt8Converter() },
        { typeof(int), new IceInt32Converter() },
        { typeof(long), new IceInt64Converter() },
        { typeof(float), new IceSingleConverter() },
        { typeof(double), new IceDoubleConverter() }
    };

    public static bool GetConverter(Type type, out IceValueConverter? converter)
    {
        var result = Converters.TryGetValue(type, out var x);
        converter = x;
        return result;
    }
}