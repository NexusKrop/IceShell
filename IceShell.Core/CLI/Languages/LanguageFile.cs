// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Core.CLI.Languages;

using Kajabity.Tools.Java;

/// <summary>
/// Represents a language file.
/// </summary>
public class LanguageFile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LanguageFile"/> class.
    /// </summary>
    /// <param name="properties">The properties file.</param>
    public LanguageFile(JavaProperties properties)
    {
        _properties = properties;
    }

    private readonly JavaProperties _properties;

    /// <inheritdoc cref="Languages.UnknownCommand(string)"/>
    public string UnknownCommand(string command)
    {
        return string.Format(_properties["shell_unknown_command"], command);
    }

    /// <inheritdoc cref="Languages.Get(string)" />
    /// <param name="key">The key of the message.</param>
    /// <returns>The message string. If the message does not exist, returns the <paramref name="key"/> instead.</returns>
    public string Get(string key)
    {
        if (!_properties.TryGetValue(key, out var value))
        {
            return key;
        }

        return value;
    }
}
