namespace NexusKrop.IceShell.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal static class ExceptionHelper
{
    public static Exception ExceptedString() => new CommandFormatException(ER.ExceptedString);

    public static Exception WithName(string message, char name) => new CommandFormatException(string.Format(message, name));
}
