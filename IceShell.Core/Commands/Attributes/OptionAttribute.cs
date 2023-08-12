namespace IceShell.Core.Commands.Attributes;
using System;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class OptionAttribute : Attribute
{
    public OptionAttribute(char character, bool hasValue, bool required = false)
    {
        Character = character;
        HasValue = hasValue;
        Required = required;
    }

    public char Character { get; set; }
    public bool HasValue { get; set; }
    public bool Required { get; set; }
}
