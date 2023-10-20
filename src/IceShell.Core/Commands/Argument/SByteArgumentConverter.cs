// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Core.Commands.Argument;

using IceShell.Core.Exceptions;
using System;
using System.Reflection;

/// <summary>
/// Provides conversion for signed 8-bit integral types.
/// </summary>
public class SByteArgumentConverter : IArgumentConverter
{
    /// <inheritdoc/>
    public void Convert(string from, PropertyInfo property, object instance)
    {
        if (property.PropertyType != typeof(sbyte))
        {
            throw new InvalidOperationException("Property is not SByte.");
        }

        if (!sbyte.TryParse(from, out var result))
        {
            throw ExceptionHelper.CannotResolve(from);
        }

        property.SetValue(instance, result);
    }
}
