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
    /// Gets an empty language file.
    /// </summary>
    public static readonly LanguageFile Empty = new(new JavaProperties());

    /// <summary>
    /// Initializes a new instance of the <see cref="LanguageFile"/> class.
    /// </summary>
    /// <param name="properties">The properties file.</param>
    public LanguageFile(JavaProperties properties)
    {
        _properties = properties;
    }

    private readonly JavaProperties _properties;

    /// <inheritdoc cref="LanguageService.UnknownCommand(string)"/>
    [Obsolete("Use LangMessage to get 'shell_unknown_command' instead.")]
    public string UnknownCommand(string command)
    {
        return string.Format(_properties["shell_unknown_command"], command);
    }

    /// <inheritdoc cref="LanguageService.Get(string)" />
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

    /// <summary>
    /// Gets the message associated with the specified key.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="result">The message, if exists.</param>
    /// <returns><see langword="true"/> if this language file contains the specified key; otherwise <see langword="false"/>.</returns>
    public bool TryGet(string key, out string? result)
    {
        return _properties.TryGetValue(key, out result);
    }

    /// <summary>
    /// Determines whether this language file contains a message with the specified key.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns><see langword="true"/> if the message exists; otherwise, <see langword="false"/>.</returns>
    public bool Contains(string key)
    {
        return _properties.ContainsKey(key);
    }
}
