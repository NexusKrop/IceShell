// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Completion.Cache;
using System.Collections.Generic;

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

        _completionEntries.Clear();

        var complete = Path.GetFullPath(dir);

        foreach (var entry in Directory.GetFileSystemEntries(complete))
        {
            _completionEntries.Add(Path.GetRelativePath(complete, entry));
        }
    }

    internal string[] Complete(string text)
    {
        var results = new List<string>(_completionEntries.Count / 2);

        var nows = string.IsNullOrWhiteSpace(text);

        foreach (var entry in _completionEntries)
        {
            var fileEntry = $".\\{entry}";

            if (nows || entry == text)
            {
                results.Add(fileEntry);
                break;
            }

            if (entry.StartsWith(text))
            {
                results.Add(fileEntry);
            }
        }

        return results.ToArray();
    }
}
