// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Core.Commands.Argument;
using System.Reflection;

/// <summary>
/// Supports string arguments.
/// </summary>
internal class StringArgumentConverter : IArgumentConverter
{
    public void Convert(string from, PropertyInfo property, object instance)
    {
        property.SetValue(instance, from);
    }
}
