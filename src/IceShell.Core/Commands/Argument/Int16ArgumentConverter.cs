// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Core.Commands.Argument;

using IceShell.Core.Exceptions;
using System;
using System.Reflection;

/// <summary>
/// Provides conversion for 16-bit integral types.
/// </summary>
public class Int16ArgumentConverter : IArgumentConverter
{
    /// <inheritdoc/>
    public void Convert(string from, PropertyInfo property, object instance)
    {
        if (property.PropertyType != typeof(short))
        {
            throw new InvalidOperationException("Property is not Int16.");
        }

        if (!short.TryParse(from, out var result))
        {
            throw ExceptionHelper.CannotResolve(from);
        }

        property.SetValue(instance, result);
    }
}
