using System.CommandLine;
using NexusKrop.IceCube;
using NexusKrop.IceShell.Core.Commands.Nove;

namespace NexusKrop.IceShell.Core.Commands;

public class NoveCommandManager
{
    internal NoveCommandManager() { }

    internal void AddDefaults()
    {
        RegisterCommand(new NoveHelpCommand());
    }

    private readonly Dictionary<string, Command> _commands = new();

    internal bool Any()
    {
        return _commands.Count != 0;
    }

    internal void ForEach(Action<Command> command)
    {
        _commands.ForEach(x => command.Invoke(x.Value));
    }

    public bool TryGetCommand(string name, out Command? command)
    {
        var success = _commands.TryGetValue(name, out var retVal);
        command = retVal;
        return success;
    }

    public void RegisterCommand(INoveCommandBuilder builder)
    {
        RegisterCommand(builder.Build());
    }

    public void RegisterCommand(Command command)
    {
        if (_commands.ContainsKey(command.Name))
        {
            throw new ArgumentException("Command already exists", nameof(command));
        }

        _commands.Add(command.Name, command);
    }
}