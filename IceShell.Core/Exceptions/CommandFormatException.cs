// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Exceptions;
using System;
using System.Runtime.Serialization;

/// <summary>
/// An exception that is thrown when the shell or a command found a syntax error or invalid input, or otherwise a 
/// runtime error or failure which is caused by invalid, malformed, wrongful or otherwise bad user input.
/// </summary>
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
