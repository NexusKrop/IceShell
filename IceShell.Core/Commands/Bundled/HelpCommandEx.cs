// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace NexusKrop.IceShell.Core.Commands.Bundled;

using NexusKrop.IceShell.Core.Commands.Complex;
using Spectre.Console;

[ComplexCommand("help", "Provides help information for IceShell commands.")]
public class HelpCommandEx : IComplexCommand
{
    public void Define(ComplexArgument argument)
    {
        argument.AddValue("command");
    }

    public int Execute(ComplexArgumentParseResult argument, Shell shell)
    {
        if (argument.Values.Count == 0)
        {
            return ExecuteSummary();
        }

        var commandName = argument.Values[0];

        if (string.IsNullOrWhiteSpace(commandName))
        {
            return ExecuteSummary();
        }

        return ExecuteDetailed(commandName);
    }

    private static int ExecuteDetailed(string commandName)
    {
        var commandType = Shell.CommandManager.GetComplex(commandName);

        if (commandType == null)
        {
            System.Console.WriteLine("No help entry for this command. Did you mean \"{0} /?\" or \"{0} --help\"?", commandName ?? "");
            return 1;
        }

        var def = new ComplexArgument(new CommandParser());

        try
        {
            var instance = (IComplexCommand)Activator.CreateInstance(commandType)!;
            instance.Define(def);
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Failed to create help definition for command {0}", commandName);
            System.Console.WriteLine(ex);
            return 2;
        }

        System.Console.WriteLine("Usage: {0}", def.GetUsage(commandName));
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
