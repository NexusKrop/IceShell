// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Core.Commands.Bundled;

using NexusKrop.IceShell.Core;
using NexusKrop.IceShell.Core.Commands.Complex;

[ComplexCommand("prompt", "Changes the IceShell command prompt.")]
public class PromptCommandEx : IComplexCommand
{
    public void Define(ComplexArgument argument)
    {
        argument.AddValue("prompt", false);
    }

    public int Execute(ComplexArgumentParseResult argument, Shell shell)
    {
        if (argument.Values.Count != 1)
        {
            // Reset prompt to default prompt

            shell.Prompt = Shell.DefaultPrompt;
            return 0;
        }

        var prompt = argument.Values[0];

        if (string.IsNullOrWhiteSpace(prompt))
        {
            // Let's do the same

            shell.Prompt = Shell.DefaultPrompt;
            return 0;
        }

        shell.Prompt = prompt;
        return 0;
    }
}
