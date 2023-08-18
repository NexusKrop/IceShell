// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Bundled;

using global::IceShell.Core;
using global::IceShell.Core.Commands;
using global::IceShell.Core.Commands.Attributes;
using NexusKrop.IceShell.Core.Commands.Complex;
using Spectre.Console;

[ComplexCommand("help", "Provides help information for IceShell commands.")]
public class HelpCommandEx : ICommand
{
    [Value("command", false, position: 0)]
    public string? CommandName { get; set; }

    public int Execute(IShell shell, ICommandExecutor executor)
    {
        var commandName = CommandName;

        if (string.IsNullOrWhiteSpace(commandName))
        {
            return ExecuteSummary();
        }

        return ExecuteDetailed(commandName);
    }

    private static int ExecuteDetailed(string commandName)
    {
        var commandType = Shell.CommandManager.GetDefinition(commandName);

        if (commandType == null)
        {
            System.Console.WriteLine("No help entry for this command. Did you mean \"{0} /?\" or \"{0} --help\"?", commandName ?? "");
            return 1;
        }

        var def = commandType.Definition;

        def.PrintHelp(commandName);
        return 0;
    }

    private int ExecuteSummary()
    {
        if (!Shell.CommandManager.CommandEntries.Any())
        {
            System.Console.WriteLine("No registered commands.");
            return 0;
        }

        System.Console.WriteLine("For more information on a specific command, type \"help <command>\"");

        var grid = new Grid();
        grid.AddColumn();
        grid.AddColumn();
        grid.AddRow(" ", " ");

        var keysEnumerable = Shell.CommandManager.CommandEntries.Keys;
        var keys = new List<string>(keysEnumerable);
        keys.Sort();

        foreach (var x in keys)
        {
            var item = Shell.CommandManager.CommandEntries[x];

            grid.AddRow(x, item.Description ?? "No description available.");
        }

        AnsiConsole.Write(grid);

        return 0;
    }
}
