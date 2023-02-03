namespace NexusKrop.IceShell.Core.Completion.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

internal class DirCache
{
    private readonly FileSystemWatcher _watcher = new();
    private readonly HashSet<string> _completionEntries = new();

    public DirCache()
    {
    }

    public DirCache(string dir)
    {
        UpdateDirectory(dir);
    }

    public void UpdateDirectory(string dir)
    {
        _watcher.Path = dir;

        foreach (var entry in Directory.GetFileSystemEntries(dir))
        {
            _completionEntries.Add(Path.GetRelativePath(dir, entry));
        }
    }

    internal string[] Complete(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return _completionEntries.ToArray();
        }

        var results = new List<string>(_completionEntries.Count / 2);

        foreach (var entry in _completionEntries)
        {
            if (entry == text)
            {
                results.Add(entry);
                break;
            }

            if (entry.StartsWith(text))
            {
                results.Add(entry);
            }
        }

        return results.ToArray();
    }
}
