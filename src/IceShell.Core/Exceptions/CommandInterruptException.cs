// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Core.Exceptions;

using IceShell.Core.Api;
using System;
using System.Runtime.Serialization;

/// <summary>
/// An exception that is thrown to interrupt the command execution. Command executors must be aware of this exception.
/// </summary>
[Serializable]
public class CommandInterruptException : Exception
{
    /// <summary>
    /// Gets the result associated with this exception.
    /// </summary>
    public CommandResult Result { get; } = CommandResult.Ok();

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandInterruptException"/> class.
    /// </summary>
    public CommandInterruptException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandInterruptException"/> class.
    /// </summary>
    /// <param name="message">The message to use.</param>
    public CommandInterruptException(string? message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandInterruptException"/> class.
    /// </summary>
    /// <param name="result">The result associated with this exception.</param>
    public CommandInterruptException(CommandResult result) : base(result.Message ?? "Command was interrupted.")
    {
        Result = result;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandInterruptException"/> class.
    /// </summary>
    /// <param name="message">The message to use.</param>
    /// <param name="innerException">The internal exception.</param>
    public CommandInterruptException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandInterruptException"/> class.
    /// </summary>
    /// <param name="info">Serialization information.</param>
    /// <param name="context">Serialization context.</param>
    protected CommandInterruptException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
