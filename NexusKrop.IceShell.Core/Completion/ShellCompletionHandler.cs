namespace NexusKrop.IceShell.Core.Completion;

using NexusKrop.IceCube;
using NexusKrop.IceShell.Core.Commands;
using NexusKrop.IceShell.Core.Completion.Cache;
using ReadLineReboot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ShellCompletionHandler : IAutoCompleteHandler
{
    internal ShellCompletionHandler(CommandManager manager, DirCache cache)
    {
        _manager = manager;
        _cache = cache;
    }

    private readonly CommandManager _manager;
    private readonly DirCache _cache;

    public char[] Separators { get; set; } = new char[] { ' ', '.', '\\' };

    public string[] GetSuggestions(string text, int index)
    {
        var commands = _manager.CompleteCommand(text);

        if (commands?.IsEmpty() != false)
        {
            return _cache.Complete(text);
        }

        return commands;
    }
}
