namespace IceShell.Core.Commands.Argument;
using System.Reflection;

/// <summary>
/// Supports string arguments.
/// </summary>
internal class StringConverter : IArgumentConverter
{
    public void Convert(string from, PropertyInfo property, object instance)
    {
        property.SetValue(instance, from);
    }
}
