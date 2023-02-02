namespace NexusKrop.IceShell.Core.Completion;

using NexusKrop.IceShell.Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ShellCompletionHandler : IAutoCompleteHandler
{
    internal ShellCompletionHandler(CommandManager manager)
    {
        _manager = manager;
    }

    private readonly CommandManager _manager;

    public char[] Separators { get; set; } = new char[] { ' ', '.', '\\' };

    public string[] GetSuggestions(string text, int index)
    {
        return null; //TODO fix
    }
}
