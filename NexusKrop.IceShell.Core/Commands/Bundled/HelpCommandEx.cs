using NexusKrop.IceShell.Core.Commands.Complex;

namespace NexusKrop.IceShell.Core.Commands.Bundled;

[ComplexCommand("help")]
public class HelpCommandEx : IComplexCommand
{
    public void Define(ComplexArgument argument)
    {
        argument.AddValue("command");
    }

    public int Execute(ComplexArgumentParseResult argument)
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
        System.Console.WriteLine("TODO");
        return 0;
    }
}
