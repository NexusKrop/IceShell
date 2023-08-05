namespace NexusKrop.IceCube.Settings;

using IniParser;
using IniParser.Model;

public class ShellSettings
{
    public bool DisplayDateTimeOnStartup { get; set; }
    public bool UseCustomPathSystem { get; set; }
    public string Language { get; set; } = "en";

    public void Save(string file)
    {
        var parser = new FileIniDataParser();

        var data = new IniData();
        data["Shell"]["DisplayDateTimeOnStartup"] = DisplayDateTimeOnStartup.ToString();
        data["Shell"][nameof(UseCustomPathSystem)] = UseCustomPathSystem.ToString();
        data["Shell"][nameof(Language)] = Language;

        parser.WriteFile(file, data);
    }

    public static ShellSettings LoadFromFile(string file)
    {
        var parser = new FileIniDataParser();

        var data = parser.ReadFile(file);

        var result = new ShellSettings();

        // Add data here
        result.DisplayDateTimeOnStartup = bool.Parse(data["Shell"]["DisplayDateTimeOnStartup"]);
        result.UseCustomPathSystem = bool.Parse(data["Shell"][nameof(UseCustomPathSystem)]);
        result.Language = data["Shell"][nameof(Language)];

        return result;
    }
}