// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Core.CLI.Languages;

using Kajabity.Tools.Java;

public class LanguageFile
{
    public LanguageFile(JavaProperties properties)
    {
        _properties = properties;
    }

    private readonly JavaProperties _properties;

    public string UnknownCommand(string command)
    {
        return string.Format(_properties["shell_unknown_command"], command);
    }

    public string Get(string key)
    {
        return _properties[key];
    }
}
