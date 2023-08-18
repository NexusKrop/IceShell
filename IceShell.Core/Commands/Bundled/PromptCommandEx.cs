// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Core.Commands.Bundled;

using IceShell.Core.Commands.Attributes;
using NexusKrop.IceShell.Core;
using NexusKrop.IceShell.Core.Commands.Complex;

[ComplexCommand("prompt", "Changes the IceShell command prompt.")]
[GreedyString]
public class PromptCommandEx : ICommand
{
    [Value("prompt", false, 0)]
    public string? Prompt { get; set; }

    public int Execute(IShell shell, ICommandExecutor executor)
    {
        var prompt = Prompt;

        if (string.IsNullOrWhiteSpace(prompt))
        {
            // Reset prompt.

            shell.Prompt = Shell.DefaultPrompt;
            return 0;
        }

        shell.Prompt = prompt;
        return 0;
    }
}
