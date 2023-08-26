// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Core.CLI.Languages;

using System.Reflection;
using Kajabity.Tools.Java;
using global::IceShell.Settings;

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

    public void Reload()
    {
        LanguageFiles.Clear();
        Load();
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

    public static string Get(string messageName)
    {
        return Instance.Current().Get(messageName);
    }

    public static string UnknownCommand(string command)
    {
        return Instance.Current().UnknownCommand(command);
    }

    public static string FormatMessage(string messageName, params object[] args)
    {
        return string.Format(Instance.Current().Get(messageName), args);
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

    public static string ArgumentSurpassingCount(int current, int max)
    {
        return string.Format(Instance.Current().Get("argument_surpassing_count"), current, max);
    }

    public static string ArgumentLowerThanCount(int current, int max)
    {
        return string.Format(Instance.Current().Get("argument_lower_than_count"), current, max);
    }

    public static string DirDirectoryOf(string directory) => FormatMessage("dir_directory_of", directory);

    public static string DelPrompt(string file) => string.Format(Get("del_prompt"), file);

    public static string UnauthorizedFile(string file) => string.Format(Get("generic_unauthorized_file"), file);
    public static string RequiresValue(int value) => string.Format(Get("generic_requires_value"), value);
    public static string CopyIsFileMore() => Get("copy_destination_file_more");
}
