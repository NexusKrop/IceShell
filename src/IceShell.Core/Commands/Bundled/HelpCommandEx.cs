// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Bundled;

using global::IceShell.Core;
using global::IceShell.Core.CLI.Languages;
using global::IceShell.Core.Commands;
using global::IceShell.Core.Commands.Attributes;
using NexusKrop.IceShell.Core.Commands.Complex;
using Spectre.Console;

/// <summary>
/// Provides help information for IceShell commands.
/// </summary>
[ComplexCommand("help", "Provides help information for IceShell commands.")]
public class HelpCommandEx : ICommand
{
    /// <summary>
    /// Gets or sets the name of the command that the user wants help information.
    /// </summary>
    /// <value>
    /// The name of the command that the user wants help information. If <see langword="null"/>, prints a list of commands.
    /// </value>
    [Value("command", false, position: 0)]
    public string? CommandName { get; set; }

    /// <inheritdoc/>
    public int Execute(IShell shell, ICommandExecutor executor, ExecutionContext context, out TextReader? pipeStream)
    {
        pipeStream = null;

        var commandName = CommandName;

        if (string.IsNullOrWhiteSpace(commandName))
        {
            return ExecuteSummary(shell);
        }

        return ExecuteDetailed(shell, commandName);
    }

    private static int ExecuteDetailed(IShell shell, string commandName)
    {
        var commandType = shell.Dispatcher.CommandManager.GetDefinition(commandName);

        if (commandType == null)
        {
            Console.WriteLine(LangMessage.Get("help_not_found"), commandName ?? "");
            return 1;
        }

        var def = commandType.Definition;

        def.PrintHelp(commandName);
        return 0;
    }

    private static int ExecuteSummary(IShell shell)
    {
        if (!shell.Dispatcher.CommandManager.Any())
        {
            Console.WriteLine(LangMessage.Get("help_no_commands"));
            return 0;
        }

        Console.WriteLine(LangMessage.Get("help_summary_more_information"));

        var grid = new Grid();
        grid.AddColumn();
        grid.AddColumn();
        grid.AddRow(" ", " ");

        var keysEnumerable = shell.Dispatcher.CommandManager.CommandAliases;
        var keys = new List<string>(keysEnumerable);
        var noDescMessage = LangMessage.Get("help_no_description");
        keys.Sort();

        foreach (var x in keys)
        {
            var item = shell.Dispatcher.CommandManager.GetDefinition(x);

            if (item == null)
            {
                continue;
            }

            grid.AddRow(x, item.Description ?? noDescMessage);
        }

        AnsiConsole.Write(grid);

        return 0;
    }
}
