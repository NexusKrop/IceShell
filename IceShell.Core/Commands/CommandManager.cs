// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands;

using global::IceShell.Core.CLI.Languages;
using global::IceShell.Core.Commands;
using global::IceShell.Core.Commands.Attributes;
using global::IceShell.Core.Commands.Bundled;
using global::IceShell.Core.Commands.Complex;
using global::IceShell.Core.Exceptions;
using NexusKrop.IceCube;
using NexusKrop.IceShell.Core.Commands.Bundled;
using NexusKrop.IceShell.Core.Commands.Complex;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;

/// <summary>
/// Provides services for the registration and lookup for the registered commands.
/// </summary>
public class CommandManager
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CommandManager"/> class.
    /// </summary>
    /// <param name="registerDefaults">If <see langword="true"/>, registers the bundled commands.</param>
    public CommandManager(bool registerDefaults = true)
    {
        CommandEntries = new ReadOnlyDictionary<string, ComplexCommandEntry>(_complexCommands);

        if (registerDefaults)
        {
            RegisterComplex(typeof(DirCommandEx));
            RegisterComplex(typeof(EchoCommandEx));
            RegisterComplex(typeof(ExitCommandEx));
            RegisterComplex(typeof(CdCommandEx));
            RegisterComplex(typeof(CopyCommandEx));
            RegisterComplex(typeof(ShellVersionCommand));
            RegisterComplex(typeof(StartCommandEx));
            RegisterComplex(typeof(ClearScreenCommandEx));
            RegisterComplex(typeof(MakeDirectoryCommand));
            RegisterComplex(typeof(DeleteFileCommandEx));
            RegisterComplex(typeof(MakeFileCommand));
            RegisterComplex(typeof(MoveCommandEx));
            RegisterComplex(typeof(HelpCommandEx));
            RegisterComplex(typeof(TypeCommandEx));
            RegisterComplex(typeof(PromptCommandEx));
        }
    }

    /// <summary>
    /// Represents a command registration entry.
    /// </summary>
    /// <param name="Type">The type containing the implementation of the command.</param>
    /// <param name="OSPlatform">The platform that the command explicitly supports. If empty, the command is considered to work on all platforms.</param>
    /// <param name="Definition">The command definition.</param>
    /// <param name="Description">The description to show in help messages.</param>
    public sealed record ComplexCommandEntry(Type Type, string[] OSPlatform, CommandDefinition Definition, string? Description = null);

    private readonly Dictionary<string, ComplexCommandEntry> _complexCommands = new();

    /// <summary>
    /// Gets the registered command entries.
    /// </summary>
    public IReadOnlyDictionary<string, ComplexCommandEntry> CommandEntries { get; }

    /// <summary>
    /// Returns a list of the command names that begins with the specified characters.
    /// </summary>
    /// <param name="begin">The characters to search for completion.</param>
    /// <returns>The list of command names.</returns>
    public string[] CompleteCommand(string begin)
    {
        if (string.IsNullOrWhiteSpace(begin))
        {
            return Array.Empty<string>();
        }

        var list = new List<string>(_complexCommands.Count);

        foreach (var command in _complexCommands.Keys)
        {
            if (command.Equals(begin))
            {
                list.Add(command);
                break;
            }

            if (command.StartsWith(begin))
            {
                list.Add(command);
            }
        }

        return list.ToArray();
    }

    /// <summary>
    /// Gets a command definition based on the name of the command.
    /// </summary>
    /// <param name="name">The name of the command. Can be an alias.</param>
    /// <returns>If found, the command entry; otherwise, <see langword="null"/>.</returns>
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

    /// <summary>
    /// Determines whether a definition with the specified name exists.
    /// </summary>
    /// <param name="name">The name to check.</param>
    /// <returns><see langword="true"/> if definition exists; otherwise, <see langword="false"/>.</returns>
    public bool HasDefinition(string name)
    {
        return _complexCommands.ContainsKey(name.ToUpperInvariant());
    }

    /// <summary>
    /// Gets a command implementation type.
    /// </summary>
    /// <param name="name">The name of the command. Can be an alias.</param>
    /// <returns>The command implementation type, if found; otherwise, <see langword="null"/>.</returns>
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

    /// <summary>
    /// Registers a command.
    /// </summary>
    /// <param name="type">The command implementation type.</param>
    /// <exception cref="ArgumentException">The command implementation type or its set of attributes are invalid.</exception>
    /// <exception cref="InvalidOperationException">The command implementation type is invalid.</exception>
    /// <remarks>
    /// <para>
    /// You will need to add a correct set of attributes to the properties and the type itself in order for it to correctly register and function.
    /// For more information, consult the documentation or the IceShell source code.
    /// </para>
    /// </remarks>
    public void RegisterComplex(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        var platforms = new List<string>();

        // Step 1: Search command attributes
        var attributes = type.GetCustomAttributes(typeof(ComplexCommandAttribute), false);

        if (attributes.Length != 1)
        {
            throw new ArgumentException(Languages.FormatMessage("api_more_than_one_attribute", nameof(ComplexCommandAttribute), type.FullName ?? type.Name), nameof(type));
        }

        // Step 2: Search platform attributes
        foreach (var attr in type.GetCustomAttributes(typeof(SupportedOSPlatformAttribute), false))
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
            throw new InvalidOperationException(string.Format(Languages.Get("api_var_values_no_buffer"), type.FullName));
        }

        // Now begin registering
        var intf = type.GetInterface(nameof(ICommand));

        if (intf != typeof(ICommand))
        {
            throw ExceptionHelper.CommandNoInterface(type, nameof(ICommand));
        }

        if (attributes[0] is not ComplexCommandAttribute attribute)
        {
            throw new ArgumentException(string.Format(Languages.Get("api_command_invalid_attribute"), type.FullName), nameof(type));
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
        var commandAttr = type.GetCustomAttribute<ComplexCommandAttribute>()!;

        if (!string.IsNullOrWhiteSpace(commandAttr.CustomUsage))
        {
            definition.CustomUsage = commandAttr.CustomUsage;
        }

        if (!string.IsNullOrWhiteSpace(commandAttr.Description))
        {
            definition.Description = commandAttr.Description;
        }

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
