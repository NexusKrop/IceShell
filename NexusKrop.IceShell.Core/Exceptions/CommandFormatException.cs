namespace NexusKrop.IceShell.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class CommandFormatException : Exception
{
    public CommandFormatException()
    {
    }

    public CommandFormatException(string? message) : base(message)
    {
    }

    public CommandFormatException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected CommandFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
