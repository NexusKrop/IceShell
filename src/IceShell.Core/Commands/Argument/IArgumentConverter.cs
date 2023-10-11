// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Core.Commands.Argument;

using System.Reflection;

/// <summary>
/// Defines an interface that converts string value to other types.
/// </summary>
public interface IArgumentConverter
{
    /// <summary>
    /// Converts the specified string.
    /// </summary>
    /// <param name="from">The string to convert.</param>
    /// <param name="property">The property to store the conversion result to.</param>
    /// <param name="instance">The instance of the command.</param>
    public void Convert(string from, PropertyInfo property, object instance);
}
