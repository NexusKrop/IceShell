// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Core.Commands.Argument;

using IceShell.Core.Exceptions;
using System.Reflection;

/// <summary>
/// Provides conversion service for 64-bit integral types.
/// </summary>
public class Int64ArgumentConverter : IArgumentConverter
{
    /// <inheritdoc/>
    public void Convert(string from, PropertyInfo property, object instance)
    {
        if (property.PropertyType != typeof(long))
        {
            throw new InvalidOperationException("Property is not Int64.");
        }

        if (!long.TryParse(from, out var result))
        {
            throw ExceptionHelper.CannotResolve(from);
        }

        property.SetValue(instance, result);
    }
}
