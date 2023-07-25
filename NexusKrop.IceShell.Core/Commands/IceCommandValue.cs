namespace NexusKrop.IceShell.Core.Commands;

public abstract class IceCommandValue
{
    public string? Description { get; set; }

    public abstract bool IsRequired();
}

public class IceCommandValue<T> : IceCommandValue
{
    private readonly Func<T?>? _getDefaultValue;

    public IceCommandValue(string? description, Func<T?>? getDefaultValue)
    {
        this._getDefaultValue = getDefaultValue;
        Description = description;
    }

    public override bool IsRequired()
    {
        return _getDefaultValue != null;
    }
}