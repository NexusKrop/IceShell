namespace NexusKrop.IceShell.Core.Commands;

using NexusKrop.IceShell.Core.Commands.Bundled;
using NexusKrop.IceShell.Core.Commands.Complex;
using NexusKrop.IceShell.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CommandManager
{
    internal CommandManager()
    {
        RegisterComplex(typeof(DirCommandEx));

        Register(typeof(EchoCommand));
        Register(typeof(ExitCommand));
        Register(typeof(CdCommand));
        Register(typeof(VerCommand));
    }

    public record class CommandRegistryEntry(Type CommandType, int NumArgs);

    private readonly Dictionary<string, CommandRegistryEntry> _commands = new();
    private readonly Dictionary<string, Type> _complexCommands = new();

    public Type? GetComplex(string name)
    {
        if (!_complexCommands.TryGetValue(name, out var result))
        {
            result = null;
        }

        return result;
    }

    public CommandRegistryEntry? Get(string name)
    {
        if (!_commands.TryGetValue(name, out var result))
        {
            return null;
        }

        return result;
    }

    public void RegisterComplex(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        var attributes = type.GetCustomAttributes(typeof(ComplexCommandAttribute), false);

        if (attributes.Length != 1)
        {
            throw new ArgumentException(ER.ManagerMoreThanOneAttribute, nameof(type));
        }

        var intf = type.GetInterface("IComplexCommand");

        if (intf != typeof(IComplexCommand))
        {
            throw new ArgumentException(ER.ManagerTypeNotCommand, nameof(type));
        }

        if (attributes[0] is not ComplexCommandAttribute attribute)
        {
            throw new ArgumentException(ER.ManagerInvalidAttribute, nameof(type));
        }

        _complexCommands.Add(attribute.Name, type);
    }

    public void Register(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        var attributes = type.GetCustomAttributes(typeof(CommandAttribute), false);

        if (attributes.Length != 1)
        {
            throw new ArgumentException(ER.ManagerMoreThanOneAttribute, nameof(type));
        }

        var intf = type.GetInterface("ICommand");

        if (intf != typeof(ICommand))
        {
            throw new ArgumentException(ER.ManagerTypeNotCommand, nameof(type));
        }

        if (attributes[0] is not CommandAttribute attribute)
        {
            throw new ArgumentException(ER.ManagerInvalidAttribute, nameof(type));
        }

        _commands.Add(attribute.Name, new(type, attribute.NumArgs));
    }
}
