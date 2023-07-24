namespace NexusKrop.IceCube.Settings;

using IniParser;
using IniParser.Model;

public class ShellSettings
{
    public bool DisplayDateTimeOnStartup { get; set; }

    public void Save(string file)
    {
        var parser = new FileIniDataParser();

        var data = new IniData();
        data["Shell"]["DisplayDateTimeOnStartup"] = DisplayDateTimeOnStartup.ToString();

        parser.WriteFile(file, data);
    }

    public static ShellSettings LoadFromFile(string file)
    {
        var parser = new FileIniDataParser();

        var data = parser.ReadFile(file);

        var result = new ShellSettings();

        // Add data here
        result.DisplayDateTimeOnStartup = bool.Parse(data["Shell"]["DisplayDateTimeOnStartup"]);

        return result;
    }
}