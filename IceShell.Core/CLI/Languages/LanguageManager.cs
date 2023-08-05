namespace IceShell.Core.CLI.Languages;

using System.Reflection;
using Kajabity.Tools.Java;
using NexusKrop.IceCube.Settings;

public class Languages
{
    public Languages(ShellSettings shellSettings)
    {
        _shellSettings = shellSettings;
    }

    private ShellSettings _shellSettings;
    private readonly Dictionary<string, LanguageFile> LanguageFiles = new();

    public LanguageFile Current()
    {
        return LanguageFiles[_shellSettings.Language];
    }

    public static Languages Instance { get; internal set; } = new(new());

    public static void SetInstanceConfig(ShellSettings settings)
    {
        Instance._shellSettings = settings;
    }

    public void Load()
    {
        var targetPath = Directory.CreateDirectory(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!,
            "CLI/Languages"));

        foreach (var file in targetPath.EnumerateFiles("*.properties"))
        {
            var propFile = new JavaProperties();

            using (var stream = file.OpenRead())
            {
                propFile.Load(stream);
            }

            LanguageFiles.Add(Path.GetFileNameWithoutExtension(file.Name), new(propFile));
        }
    }

    public static string UnknownCommand(string command)
    {
        return Instance.Current().UnknownCommand(command);
    }

    public static string InvalidFile(string fileName)
    {
        return string.Format(Instance.Current().Get("shell_invalid_executable"), fileName);
    }

    public static string GenericBadDirectory(string directory)
    {
        return string.Format(Instance.Current().Get("generic_bad_directory"), directory);
    }

    public static string MakeDirFileExists(string file)
    {
        return string.Format(Instance.Current().Get("mkdir_file_exists"), file);
    }
}
