using System.CommandLine;

namespace NexusKrop.IceShell.Core.Commands;

public class NoveCommandManager
{
    internal NoveCommandManager() { }

    private readonly Dictionary<string, Command> _commands = new();

    public bool TryGetCommand(string name, out Command? command)
    {
        var success = _commands.TryGetValue(name, out var retVal);
        command = retVal;
        return success;
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