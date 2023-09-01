// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Core.Exceptions;
using System;
using System.Runtime.Serialization;

/// <summary>
/// An exception that is thrown when the shell or a command found a syntax error or invalid input, or otherwise a 
/// runtime error or failure which is caused by invalid, malformed, wrongful or otherwise bad user input.
/// </summary>
[Serializable]
public class CommandFormatException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CommandFormatException"/> class.
    /// </summary>
    public CommandFormatException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandFormatException"/> class.
    /// </summary>
    /// <param name="message">The message to use.</param>
    public CommandFormatException(string? message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandFormatException"/> class.
    /// </summary>
    /// <param name="message">The message to use.</param>
    /// <param name="innerException">The internal exception.</param>
    public CommandFormatException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandFormatException"/> class.
    /// </summary>
    /// <param name="info">Serialization information.</param>
    /// <param name="context">Serialization context.</param>
    protected CommandFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
