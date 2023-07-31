// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Complex;
/// <summary>
/// Represents an option in complex arguments.
/// </summary>
public record ComplexArgumentOption
{
    public ComplexArgumentOption(string option, string? value = null)
    {
        Option = option;
        Value = value;
    }

    public string Option { get; set; }
    public string? Value { get; set; }
}
