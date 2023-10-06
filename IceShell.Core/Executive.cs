namespace IceShell.Core;

using IceShell.Core.Commands;
using IceShell.Settings;
using NexusKrop.IceCube;
using NexusKrop.IceCube.Util.Enumerables;
using NexusKrop.IceShell.Core.FileSystem;
using ReadLineReboot;
using System.Diagnostics;

/// <summary>
/// Provides executive services for the IceShell processes.
/// </summary>
public static class Executive
{
    private static ShellSettings? _settings;

    #region Start Info handling
    internal static ProcessStartInfo CreateStartInfo(CommandSection section)
    {
        return CreateStartInfo(section.Name,
            !(section.Name.StartsWith('.') || PathSearcher.IsRooted(section.Name)),
            section.Statements?.Select(x => x.Content) ?? Enumerable.Empty<string>());
    }

    internal static ProcessStartInfo CreateStartInfo(string relativeFile, bool searchInPath, IEnumerable<string>? args)
    {
        string? actual = null;

        if (searchInPath)
        {
            actual = PathSearcher.SearchExecutable(relativeFile);
        }
        else
        {
            actual = Path.GetFullPath(relativeFile, Directory.GetCurrentDirectory());
        }

        if (actual == null || !File.Exists(actual))
        {
            throw new FileNotFoundException("Cannot find file", relativeFile);
        }

        var result = new ProcessStartInfo(actual)
        {
            UseShellExecute = false
        };

        args?.ForEach(result.ArgumentList.Add);

        return result;
    }
    #endregion

    internal static Process? WatchingProcess { get; private set; }

    internal static bool Interrupt { get; private set; }

    internal static void WatchProcess(Process process)
    {
        if (process == null)
        {
            return;
        }

        WatchingProcess = process;
    }

    /// <summary>
    /// Attaches the executive services to the current process.
    /// </summary>
    public static void Attach(ShellSettings settings)
    {
        Console.CancelKeyPress += Console_CancelKeyPress;

        _settings = settings;
    }

    private static void Console_CancelKeyPress(object? sender, ConsoleCancelEventArgs e)
    {
        if (WatchingProcess == null || WatchingProcess.HasExited)
        {
            // This is only a provisional solution because we cannot break out of ReadLine.

            Environment.Exit(0);
            return;
        }

        e.Cancel = true;

        if ((_settings?.TerminateUponInterrupt == true) && OperatingSystem.IsLinux())
        {
            WatchingProcess.EndGracefully();
        }
        else
        {
            WatchingProcess.Kill();
        }

        WatchingProcess = null;
    }
}
