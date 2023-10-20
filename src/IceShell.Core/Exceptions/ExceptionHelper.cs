// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Core.Exceptions;
using System;
using IceShell.Core.CLI.Languages;

internal static class ExceptionHelper
{
    public static Exception ExceptedString() => new CommandFormatException(LangMessage.Get("argument_string_required"));

    public static Exception WithName(string message, char name) => new CommandFormatException(string.Format(message, name));

    public static Exception WithName(string message, string? name) => new CommandFormatException(string.Format(message, name ?? "null"));

    public static Exception WithMessage(string message) => new CommandFormatException(LangMessage.Get(message));

    public static Exception UnknownEnvironmentVariable(string name) => new CommandFormatException(LangMessage.GetFormat("generic_unresolved_env_variable", name));

    public static Exception BadDirectory(string directory) => new CommandFormatException(LangMessage.MsgDirectoryNotFound(directory));

    public static Exception UnauthorizedWrite() => new CommandFormatException(LangMessage.Get("generic_unauthorized_write"));

    public static Exception CommandNoInterface(Type type, string interfaceName) => new InvalidOperationException(LangMessage.GetFormat("api_command_no_interface", nameof(type), interfaceName));

    public static Exception RequiresValue(int valueNum) => new CommandFormatException(LangMessage.MsgMissingValue(valueNum));

    public static Exception FileNotFound(string fileName) => new CommandFormatException(LangMessage.MsgFileNotFound(fileName));

    public static Exception FileAlreadyExists(string fileName) => new CommandFormatException(LangMessage.MsgFileAlreadyExists(fileName));

    public static Exception DirectoryAlreadyExists(string dirName) => new CommandFormatException(LangMessage.MsgDirectoryAlreadyExists(dirName));

    public static Exception InvalidPath() => new CommandFormatException(LangMessage.InvalidPath());

    public static Exception CannotResolve(string arg) => new CommandFormatException(LangMessage.MsgCannotResolve(arg));
}
