namespace IceShell.Core.Commands.Complex;

[AttributeUsage(AttributeTargets.Class)]
public sealed class CommandAliasAttribute : Attribute
{
    public CommandAliasAttribute(string alias)
    {
        Alias = alias;
    }

    public string Alias { get; set; }
}
