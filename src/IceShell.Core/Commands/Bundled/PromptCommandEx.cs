// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Core.Commands.Bundled;

using IceShell.Core.Api;
using IceShell.Core.Commands.Attributes;
using NexusKrop.IceShell.Core;
using NexusKrop.IceShell.Core.Commands.Complex;

/// <summary>
/// Changes the IceShell command prompt.
/// </summary>
[ComplexCommand("prompt", "Changes the IceShell command prompt.")]
[GreedyString]
public class PromptCommandEx : IShellCommand
{
    /// <summary>
    /// Gets or sets the command prompt to change to.
    /// </summary>
    /// <value>
    /// The command prompt to change to. If <see langword="null"/>, the prompt is reset to <see cref="Shell.DefaultPrompt"/>.
    /// </value>
    [Value("prompt", false, 0)]
    public string? Prompt { get; set; }

    /// <inheritdoc />
    public CommandResult Execute(IShell shell, ICommandExecutor executor, ExecutionContext context)
    {
        var prompt = Prompt;

        if (string.IsNullOrWhiteSpace(prompt))
        {
            // Reset prompt.

            shell.Prompt = Shell.DefaultPrompt;
            return CommandResult.Ok();
        }

        shell.Prompt = prompt;
        return CommandResult.Ok();
    }
}
