// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Core.CLI.Languages;

using System.Reflection;
using Kajabity.Tools.Java;
using global::IceShell.Settings;

/// <summary>
/// Provide methods and properties to help with the usage of language files.
/// </summary>
public class LanguageService
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LanguageService"/> class.
    /// </summary>
    /// <param name="shellSettings">The settings.</param>
    public LanguageService(ShellSettings shellSettings)
    {
        _shellSettings = shellSettings;
    }

    private ShellSettings _shellSettings;
    private readonly Dictionary<string, LanguageFile> _languageFiles = new();

    /// <summary>
    /// Gets the currently using language file.
    /// </summary>
    /// <returns></returns>
    public LanguageFile Current()
    {
        if (!_languageFiles.TryGetValue(_shellSettings.Language, out var value))
        {
            return LanguageFile.Empty;
        }

        return value;
    }

    /// <summary>
    /// Gets the <c>en</c> language file.
    /// </summary>
    /// <returns>The English language file.</returns>
    public LanguageFile English()
    {
        if (!_languageFiles.TryGetValue("en", out var value))
        {
            return LanguageFile.Empty;
        }

        return value;
    }

    /// <summary>
    /// Gets the current language service instance.
    /// </summary>
    public static LanguageService Instance { get; internal set; } = new(new());

    /// <summary>
    /// Sets the configuration used by the current language service instance to the specified configuration.
    /// </summary>
    /// <param name="settings">The configuration to use.</param>
    public static void SetInstanceConfig(ShellSettings settings)
    {
        Instance._shellSettings = settings;
    }

    /// <summary>
    /// Reloads all language files.
    /// </summary>
    public void Reload()
    {
        _languageFiles.Clear();
        Load();
    }

    /// <summary>
    /// Loads language files from the <c>CLI/Languages</c> directory in the executing assembly directory.
    /// </summary>
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

            _languageFiles.Add(Path.GetFileNameWithoutExtension(file.Name), new(propFile));
        }
    }

    /// <summary>
    /// Gets the message of specified name from the current language file.
    /// </summary>
    /// <param name="messageName">The name of the message.</param>
    /// <returns>The message string. If the message does not exist, returns the <paramref name="messageName"/> instead.</returns>
    [Obsolete("Use LangMessage.Get instead.")]
    public static string Get(string messageName)
    {
        return Instance.Current().Get(messageName);
    }

    /// <summary>
    /// Gets the message that is displayed when the command user have specified does not exist.
    /// </summary>
    /// <param name="command">The name of the command that user have specified.</param>
    /// <returns>The unknown command message.</returns>
    [Obsolete("Use LangMessage.MsgUnknownCommand")]
    public static string UnknownCommand(string command)
    {
        return Instance.Current().UnknownCommand(command);
    }

    /// <summary>
    /// Gets the message of specified name from the current language file and interpolate it
    /// with the specified arguments via <see cref="string.Format(string, object?[])"/>.
    /// </summary>
    /// <param name="messageName">The name of the message.</param>
    /// <param name="args">The format arguments.</param>
    /// <returns>The formatted language string.</returns>
    [Obsolete("Use LangMessage.GetFormat")]
    public static string FormatMessage(string messageName, params object[] args)
    {
        return string.Format(Instance.Current().Get(messageName), args);
    }

    /// <summary>
    /// Gets the message that is displayed when the specified file does not exist.
    /// </summary>
    /// <param name="fileName">The name of or path to the file.</param>
    /// <returns>The file not found message.</returns>
    [Obsolete("Use LangMessage.MsgInvalidExecutable")]
    public static string InvalidFile(string fileName)
    {
        return string.Format(Instance.Current().Get("shell_invalid_executable"), fileName);
    }

    /// <summary>
    /// Gets the message that is displayed when the specified directory does not exist.
    /// </summary>
    /// <param name="directory">The name of or path to directory.</param>
    /// <returns>The directory not found message.</returns>
    [Obsolete("Use LangMessage.MsgBadDirectory")]
    public static string GenericBadDirectory(string directory)
    {
        return string.Format(Instance.Current().Get("generic_bad_directory"), directory);
    }

    /// <summary>
    /// Gets the message that is displayed when there are more arguments specified than the command can accept.
    /// </summary>
    /// <param name="current">The amount of arguments that was specified.</param>
    /// <param name="max">The maximum amount of arguments that the command can accept.</param>
    /// <returns>The too many arguments message.</returns>
    [Obsolete("Use LangMessage.MsgTooManyArguments")]
    public static string ArgumentSurpassingCount(int current, int max)
    {
        return string.Format(Instance.Current().Get("argument_surpassing_count"), current, max);
    }

    /// <summary>
    /// Gets the message that is displayed when there are less arguments specified than the command requires.
    /// </summary>
    /// <param name="current">The amount of arguments that was specified.</param>
    /// <param name="max">The amount of arguments that the command requires.</param>
    /// <returns>The too less arguments message.</returns>
    [Obsolete("Use LangMessage.MsgTooFewArguments")]
    public static string ArgumentLowerThanCount(int current, int max)
    {
        return string.Format(Instance.Current().Get("argument_lower_than_count"), current, max);
    }

    [Obsolete("Acquire message dir_directory_of instead. Other commands should not use this message.")]
    internal static string DirDirectoryOf(string directory) => FormatMessage("dir_directory_of", directory);

    /// <summary>
    /// Gets the message that is displayed when user confirmation is needed before deleting a file.
    /// </summary>
    /// <param name="file">The name of or path to the file to delete.</param>
    /// <returns>The deletion confirmation message.</returns>
    [Obsolete("Acquire message del_prompt instead. Other commands should not use this message.")]
    public static string DeletePrompt(string file) => string.Format(Get("del_prompt"), file);

    /// <summary>
    /// Gets the message that is displayed when the procedure cannot continue because it does not have access
    /// to the specified file.
    /// </summary>
    /// <param name="file">The path to or the name of the file.</param>
    /// <returns>The unauthorized file access message.</returns>
    [Obsolete("Use LangMessage.MsgUnauthorizedFile")]
    public static string UnauthorizedFile(string file) => string.Format(Get("generic_unauthorized_file"), file);

    [Obsolete("Use LangMessage.MsgMissingValue")]
    internal static string RequiresValue(int value) => string.Format(Get("generic_requires_value"), value);

    [Obsolete("Acquire message copy_destination_file_more instead. Other commands should not use this message.")]
    internal static string CopyIsFileMore() => Get("copy_destination_file_more");

    [Obsolete("Use LangMessage.MsgMsgActionNeverComplete")]
    internal static string ActionNeverComplete() => Get("generic_action_never_complete");
}
