// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Bundled;

using global::IceShell.Core;
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
            // TODO LOCALISE THIS
            Console.WriteLine("No help entry for this command. Did you mean \"{0} /?\" or \"{0} --help\"?", commandName ?? "");
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
            // TODO localise this
            System.Console.WriteLine("No registered commands.");
            return 0;
        }

        // TODO localise this
        Console.WriteLine("For more information on a specific command, type \"help <command>\"");

        var grid = new Grid();
        grid.AddColumn();
        grid.AddColumn();
        grid.AddRow(" ", " ");

        var keysEnumerable = shell.Dispatcher.CommandManager.CommandAliases;
        var keys = new List<string>(keysEnumerable);
        keys.Sort();

        foreach (var x in keys)
        {
            var item = shell.Dispatcher.CommandManager.GetDefinition(x);

            if (item == null)
            {
                continue;
            }

            grid.AddRow(x, item.Description ?? "No description available.");
        }

        AnsiConsole.Write(grid);

        return 0;
    }
}
