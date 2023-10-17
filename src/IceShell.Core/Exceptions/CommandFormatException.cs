// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Core.Exceptions;
using System;
using System.Runtime.Serialization;

/// <summary>
/// An exception that is thrown when the shell found a syntax error.
/// </summary>
/// <remarks>
/// If the command encountered a runtime error, the command then should interrupt the command execution (return or throwing <see cref="CommandInterruptException"/>
/// with appropriate <see cref="IceShell.Core.Api.CommandResult"/> set.
/// </remarks>
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
