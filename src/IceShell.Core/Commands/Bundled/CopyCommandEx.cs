// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Bundled;

using global::IceShell.Core.Commands.Attributes;
using global::IceShell.Core.Commands.Bundled;
using NexusKrop.IceShell.Core.Commands.Complex;

/// <summary>
/// Copies a file to another location.
/// </summary>
[ComplexCommand("copy", "Copies a file to another location.", CustomUsage = "<sources...> <destination>")]
[VariableValue]
public class CopyCommandEx : DestinationFileCommandBase
{
    /// <inheritdoc />
    public override void DoOperation(string source, string destination)
    {
        File.Copy(source, destination);
    }
}
