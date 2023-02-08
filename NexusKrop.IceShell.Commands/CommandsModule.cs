namespace NexusKrop.IceShell.Commands;

using NexusKrop.IceShell.Core.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

public class CommandsModule : IModule
{
    public static Assembly Assembly => typeof(CommandsModule).Assembly;

    public void Initialize()
    {
        // reserved
    }
}
