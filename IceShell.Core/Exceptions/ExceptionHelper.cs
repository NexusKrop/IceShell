// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Core.Exceptions;
using System;
using IceShell.Core.CLI.Languages;

internal static class ExceptionHelper
{
    public static Exception ExceptedString() => new CommandFormatException(Languages.Get("argument_string_required"));

    public static Exception WithName(string message, char name) => new CommandFormatException(string.Format(message, name));
    public static Exception WithName(string message, string name) => new CommandFormatException(string.Format(message, name));

    public static Exception BadDirectory(string directory) => new CommandFormatException(Languages.GenericBadDirectory(directory));
    public static Exception UnauthorizedWrite() => new CommandFormatException(Languages.Get("generic_unauthorized_write"));

    public static Exception CommandNoInterface(Type type, string interfaceName) => new InvalidOperationException(string.Format(Languages.Get("api_command_no_interface"), nameof(type), interfaceName));
}
