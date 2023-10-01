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
    public bool DisplayDateTimeOnStartUp { get; set; }

    /// <summary>
    /// Gets or sets whether to display the version of the shell interpreter when it starts.
    /// </summary>
    public bool DisplayShellInfoOnStartUp { get; set; }

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
        data["Shell"]["DisplayDateTimeOnStartup"] = DisplayDateTimeOnStartUp.ToString();
        data["Shell"][nameof(Language)] = Language;
        data["Shell"][nameof(DisplayShellInfoOnStartUp)] = DisplayShellInfoOnStartUp.ToString();

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

        return LoadFrom(data);
    }

    /// <summary>
    /// Creates an instance of <see cref="ShellSettings"/> with the specified <see cref="IniData"/>.
    /// </summary>
    /// <param name="data">The data to read from.</param>
    /// <returns>An instance of <see cref="ShellSettings"/> based on the specified <see cref="IniData"/>.</returns>
    public static ShellSettings LoadFrom(IniData data)
    {
        return new ShellSettings
        {
            // Add data here
            DisplayDateTimeOnStartUp = bool.Parse(GetSettingSafe(data, "Shell", "DisplayDateTimeOnStartup", "false")),
            Language = GetSettingSafe(data, "Shell", nameof(Language), "en"),
            DisplayShellInfoOnStartUp = bool.Parse(GetSettingSafe(data, "Shell", nameof(DisplayShellInfoOnStartUp), "false"))
        };
    }

    private static string GetSettingSafe(IniData data, string key, string name, string defaultValue)
    {
        var section = data[key];

        if (!section.ContainsKey(name))
        {
            section[name] = defaultValue;
        }

        return section[name];
    }
}