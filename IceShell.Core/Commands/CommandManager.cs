// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands;

using global::IceShell.Core.Commands;
using global::IceShell.Core.Commands.Attributes;
using global::IceShell.Core.Commands.Bundled;
using global::IceShell.Core.Commands.Complex;
using NexusKrop.IceCube;
using NexusKrop.IceShell.Core.Commands.Bundled;
using NexusKrop.IceShell.Core.Commands.Complex;
using NexusKrop.IceShell.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;

public class CommandManager
{
    public CommandManager()
    {
        CommandEntries = new ReadOnlyDictionary<string, ComplexCommandEntry>(_complexCommands);

        RegisterComplex(typeof(DirCommandEx));
        RegisterComplex(typeof(EchoCommandEx));
        RegisterComplex(typeof(ExitCommandEx));
        RegisterComplex(typeof(CdCommandEx));
        RegisterComplex(typeof(CopyCommandEx));
        RegisterComplex(typeof(VerCommand));
        RegisterComplex(typeof(StartCommandEx));
        RegisterComplex(typeof(ClsCommandEx));
        RegisterComplex(typeof(MkdirCommandEx));
        RegisterComplex(typeof(DelCommandEx));
        RegisterComplex(typeof(MkfileCommandEx));
        RegisterComplex(typeof(MoveCommandEx));
        RegisterComplex(typeof(HelpCommandEx));
        RegisterComplex(typeof(TypeCommandEx));
        RegisterComplex(typeof(PromptCommandEx));
    }

    public sealed record ComplexCommandEntry(Type Type, string[] OSPlatform, CommandDefinition Definition, string? Description = null);

    private readonly Dictionary<string, ComplexCommandEntry> _complexCommands = new();

    public IReadOnlyDictionary<string, ComplexCommandEntry> CommandEntries { get; }

    public string[] CompleteCommand(string begin)
    {
        if (string.IsNullOrWhiteSpace(begin))
        {
            return Array.Empty<string>();
        }

        var list = new List<string>(_complexCommands.Count);

        foreach (var command in _complexCommands.Keys)
        {
            if (begin == command)
            {
                list.Add(command);
                break;
            }

            if (!command.Equals(begin) && command.StartsWith(begin))
            {
                list.Add(command);
            }
        }

        return list.ToArray();
    }

    public ComplexCommandEntry? GetDefinition(string name)
    {
        if (!_complexCommands.TryGetValue(name.ToUpperInvariant(), out var x))
        {
            x = null;
        }

        if (x != null && !x.OSPlatform.IsEmpty() && !x.OSPlatform.Any(platform => OperatingSystem.IsOSPlatform(platform)))
        {
            return null;
        }

        return x;
    }

    public Type? GetComplex(string name)
    {
        if (!_complexCommands.TryGetValue(name.ToUpperInvariant(), out var x))
        {
            x = null;
        }

        if (x != null && !x.OSPlatform.IsEmpty() && !x.OSPlatform.Any(platform => OperatingSystem.IsOSPlatform(platform)))
        {
            return null;
        }

        return x?.Type;
    }

    public void RegisterComplex(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        var platforms = new List<string>();

        // Step 1: Search command attributes
        var attributes = type.GetCustomAttributes(typeof(ComplexCommandAttribute), false);

        if (attributes.Length != 1)
        {
            throw new ArgumentException(ER.ManagerMoreThanOneAttribute, nameof(type));
        }

        // Step 2: Search platform attributes
        var platformAttr = type.GetCustomAttributes(typeof(SupportedOSPlatformAttribute), false);

        foreach (var attr in platformAttr)
        {
            if (attr is SupportedOSPlatformAttribute os)
            {
                platforms.Add(os.PlatformName);
            }
        }

        // Step 3: Search alias attributes
        var aliasAttr = type.GetCustomAttributes(typeof(CommandAliasAttribute), false);

        // Step 4: Create definitions
        var definition = GetDefine(type);

        // Step 5: Verify definition
        if (definition.VariableValues && (definition.VariableValueBuffer == null
            || !definition.VariableValueBuffer.PropertyType.IsAssignableFrom(typeof(ReadOnlyCollection<string>))))
        {
            throw new InvalidOperationException($"Command type {type} is a variable values command but does not have a buffer for that, or buffer is invalid");
        }

        // Now begin registering
        var intf = type.GetInterface("IComplexCommand");

        if (intf != typeof(IComplexCommand))
        {
            throw new ArgumentException(ER.ManagerTypeNotCommand, nameof(type));
        }

        if (attributes[0] is not ComplexCommandAttribute attribute)
        {
            throw new ArgumentException(ER.ManagerInvalidAttribute, nameof(type));
        }

        _complexCommands.Add(attribute.Name.ToUpperInvariant(), new(type, platforms.ToArray(), definition, attribute.Description));

        // Register all of its aliases
        foreach (var attr in aliasAttr)
        {
            if (attr is CommandAliasAttribute alias)
            {
                _complexCommands.Add(alias.Alias.ToUpperInvariant(), new(type, platforms.ToArray(), definition, attribute.Description));
            }
        }
    }

    private static CommandDefinition GetDefine(Type type)
    {
        var definition = new CommandDefinition();

        if (type.GetCustomAttribute<GreedyStringAttribute>() != null)
        {
            definition.Greedy();
        }

        if (type.GetCustomAttribute<VariableValueAttribute>() != null)
        {
            definition.VarValues();
        }

        foreach (var property in type.GetProperties())
        {
            property.GetCustomAttributes().ForEach(x =>
            {
                var isValue = false;

                if (x is VariableValueBufferAttribute)
                {
                    definition.VariableValueBuffer = property;
                    return true;
                }

                // Register values first
                if (x is ValueAttribute valAttr)
                {
                    isValue = true;
                    definition.Value(new(valAttr.Name, property, valAttr.Required), valAttr.Position);
                }

                // Register options
                if (x is OptionAttribute optAttr)
                {
                    // If is already defined as value, throw
                    if (isValue)
                    {
                        throw new InvalidOperationException("A value cannot be option and value at the same time!");
                    }

                    if (!optAttr.HasValue && property.PropertyType != typeof(bool))
                    {
                        throw new InvalidOperationException($"Attempting to register property {property.Name} without value and property is not boolean");
                    }

                    definition.Option(optAttr.Character, optAttr.HasValue, property, optAttr.Required);
                }

                return true;
            });
        }

        return definition;
    }
}
