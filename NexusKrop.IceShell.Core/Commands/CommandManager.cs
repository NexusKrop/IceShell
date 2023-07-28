﻿// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands;

using NexusKrop.IceCube;
using NexusKrop.IceShell.Core.Commands.Bundled;
using NexusKrop.IceShell.Core.Commands.Complex;
using NexusKrop.IceShell.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Versioning;

public class CommandManager
{
    internal CommandManager()
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
    }

    public sealed record ComplexCommandEntry(Type Type, string[] OSPlatform, string? Description);

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

    public Type? GetComplex(string name)
    {
        if (!_complexCommands.TryGetValue(name, out var x))
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

        var attributes = type.GetCustomAttributes(typeof(ComplexCommandAttribute), false);

        if (attributes.Length != 1)
        {
            throw new ArgumentException(ER.ManagerMoreThanOneAttribute, nameof(type));
        }

        var platformAttr = type.GetCustomAttributes(typeof(SupportedOSPlatformAttribute), false);

        foreach (var attr in platformAttr)
        {
            if (attr is SupportedOSPlatformAttribute os)
            {
                platforms.Add(os.PlatformName);
            }
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

        _complexCommands.Add(attribute.Name, new(type, platforms.ToArray(), attribute.Description));
    }
}
