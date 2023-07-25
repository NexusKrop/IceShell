using NexusKrop.IceShell.Core.Commands.Conversion;

namespace NexusKrop.IceShell.Core.Commands;

public abstract class IceCommandOption
{
    protected IceCommandOption(string name, string? description = null)
    {
        Name = name;
        Description = description;
    }

    public string Name { get; set; }
    public string? Description { get; set; }

    public abstract bool IsRequired();
    public abstract bool TryProduce(string text, out object? value);
}

public class IceCommandSwitch : IceCommandOption
{
    public IceCommandSwitch(string name, string? description = null) : base(name, description)
    {
    }

    public override bool IsRequired()
    {
        return false;
    }

    public override bool TryProduce(string text, out object? value)
    {
        if (text != null && text != "")
        {
            value = null;
            return false;
        }

        value = text != null;
        return true;
    }
}

public class IceCommandOption<T> : IceCommandOption
{
    private readonly Func<T?>? _getDefaultValue;

    public IceCommandOption(string name, string? description = null, Func<T?>? getDefaultValue = null)
        : base(name, description)
    {
        _getDefaultValue = getDefaultValue;
    }

    public override bool IsRequired()
    {
        return _getDefaultValue != null;
    }

    public override bool TryProduce(string text, out object? value)
    {
        if (!IceConvertService.GetConverter(typeof(T), out var converter)
            || converter == null)
        {
            value = null;
            return false;
        }

        var result = converter.TryConvert(text, out var x);
        value = x;
        return result;
    }
}

