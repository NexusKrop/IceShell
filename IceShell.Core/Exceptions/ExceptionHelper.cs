// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Exceptions;
using System;
using global::IceShell.Core.CLI.Languages;

internal static class ExceptionHelper
{
    public static Exception ExceptedString() => new CommandFormatException(ER.ExceptedString);

    public static Exception WithName(string message, char name) => new CommandFormatException(string.Format(message, name));
    public static Exception WithName(string message, string name) => new CommandFormatException(string.Format(message, name));

    public static Exception BadDirectory(string directory) => new CommandFormatException(Languages.GenericBadDirectory(directory));
}
