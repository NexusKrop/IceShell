namespace NexusKrop.IceShell.Core.Commands.Bundled;

using NexusKrop.IceCube;
using NexusKrop.IceShell.Core.Commands.Complex;
using NexusKrop.IceShell.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[ComplexCommand("start")]
public class StartCommandEx : IComplexCommand
{
    public void Define(ComplexArgument argument)
    {
        argument.AddValue(new("target", true));
    }

    public void Execute(ComplexArgumentParseResult argument)
    {
        var proc = argument.Values[0]!;

        if (!File.Exists(proc))
        {
            throw new CommandFormatException(Messages.BadFileGeneric);
        }

        try
        {
            ProcessUtil.ShellExecute(argument.Values[0]!);
        }
        catch (Win32Exception x) when (x.NativeErrorCode == 1155)
        {
            throw new CommandFormatException(Messages.BadFileHandler);
        }
    }
}
