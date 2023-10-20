// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Core.Commands.Argument;

using IceShell.Core.Api;
using IceShell.Core.Exceptions;
using NexusKrop.IceShell.Core.CLI;
using System;
using System.Collections.Generic;
using System.Reflection;

internal class ArgumentConvertService
{
    private readonly Dictionary<Type, IArgumentConverter> _converters = new();

    internal ArgumentConvertService RegisterConverter(Type type, IArgumentConverter converter)
    {
        _converters.Add(type, converter);
        return this;
    }

    internal void UnregisterConverter(Type type)
    {
        _converters.Remove(type);
    }

    internal bool TryConvert(string from, PropertyInfo target, IShellCommand instance)
    {
        var targetType = target.PropertyType;

        if (targetType.IsEnum)
        {
            targetType = typeof(Enum);
        }

        if (!_converters.TryGetValue(targetType, out var converter))
        {
            return false;
        }

        try
        {
            converter.Convert(from, target, instance);
        }
        catch (CommandFormatException ex)
        {
            ConsoleOutput.PrintShellError(ex.Message);
            return false;
        }

        return true;
    }
}
