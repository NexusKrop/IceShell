// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Core.Commands.Argument;

using IceShell.Core.Exceptions;
using System.Reflection;

/// <summary>
/// Supports enumeration argument conversion.
/// </summary>
public class EnumerationConverter : IArgumentConverter
{
    /// <inheritdoc />
    /// <exception cref="InvalidOperationException">The property is not an enumeration.</exception>
    public void Convert(string from, PropertyInfo property, object instance)
    {
        if (!property.PropertyType.IsEnum)
        {
            throw new InvalidOperationException("The property is not an enumeration.");
        }

        var type = property.PropertyType;

        if (!Enum.TryParse(type, from, true, out var result))
        {
            throw ExceptionHelper.WithMessage("Invalid value");
        }

        property.SetValue(instance, result);
    }
}
