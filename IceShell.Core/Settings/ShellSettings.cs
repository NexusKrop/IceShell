// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Settings;

using IniParser;
using IniParser.Model;

/// <summary>
/// Configuration of a standard IceShell interpreter.
/// </summary>
public class ShellSettings
{
    /// <summary>
    /// Gets or sets whether to display date and time when the shell interpreter starts.
    /// </summary>
    public bool DisplayDateTimeOnStartup { get; set; }

    /// <summary>
    /// Gets or sets whether to use a custom, platform-agnostic (to user only) path system.
    /// </summary>
    public bool UseCustomPathSystem { get; set; }

    /// <summary>
    /// Gets or sets the language file to use.
    /// </summary>
    public string Language { get; set; } = "en";

    /// <summary>
    /// Writes the configuration to the specified file. The existing contents of the file will be deleted.
    /// </summary>
    /// <param name="file">The file to write to.</param>
    public void Save(string file)
    {
        var parser = new FileIniDataParser();

        var data = new IniData();
        data["Shell"]["DisplayDateTimeOnStartup"] = DisplayDateTimeOnStartup.ToString();
        data["Shell"][nameof(UseCustomPathSystem)] = UseCustomPathSystem.ToString();
        data["Shell"][nameof(Language)] = Language;

        parser.WriteFile(file, data);
    }

    /// <summary>
    /// Creates an instance of <see cref="ShellSettings"/> based on the contents of the specified file.
    /// </summary>
    /// <param name="file">The file to read from.</param>
    /// <returns>An instance of <see cref="ShellSettings"/> based on the contents of the specified file.</returns>
    public static ShellSettings LoadFromFile(string file)
    {
        var parser = new FileIniDataParser();

        var data = parser.ReadFile(file);

        var result = new ShellSettings
        {
            // Add data here
            DisplayDateTimeOnStartup = bool.Parse(data["Shell"]["DisplayDateTimeOnStartup"]),
            UseCustomPathSystem = bool.Parse(data["Shell"][nameof(UseCustomPathSystem)]),
            Language = data["Shell"][nameof(Language)]
        };

        return result;
    }
}