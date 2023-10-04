namespace IceShell.Core;

using IceShell.Core.Commands;
using NexusKrop.IceCube.Util.Enumerables;
using NexusKrop.IceShell.Core.FileSystem;
using System.Diagnostics;

internal static class Executive
{
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
}
