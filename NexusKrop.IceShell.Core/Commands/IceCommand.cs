namespace NexusKrop.IceShell.Core.Commands;

public class IceCommand
{
    public IceCommand(string name, string? description = null)
    {
        this.Name = name;
        this.Description = description;
    }

    public string Name { get; set; }
    public string? Description { get; set; }

    internal List<IceCommandOption> Options { get; } = new();
    internal List<IceCommandValue> Values { get; } = new();

    public void Add(IceCommandOption option)
    {
        Options.Add(option);
    }

    public void Add(IceCommandValue value)
    {
        Values.Add(value);
    }
}
