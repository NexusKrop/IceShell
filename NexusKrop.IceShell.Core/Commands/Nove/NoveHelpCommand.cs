using System.CommandLine;
using System.CommandLine.Invocation;

namespace NexusKrop.IceShell.Core.Commands.Nove;

public class NoveHelpCommand : INoveCommandBuilder
{
    private static readonly Argument<string?> _argCommand = new("command", () => null, "The command to enquiry");

    public Command Build()
    {
        var retVal = new Command("help", "Get help")
        {
            _argCommand
        };

        retVal.SetHandler(ExecuteCommand);
        return retVal;
    }

    private static void ExecuteCommand(InvocationContext context)
    {
        if (!Shell.NoveCommandManager.Any())
        {
            return;
        }

        var argCommand = context.ParseResult.GetValueForArgument(_argCommand);

        if (string.IsNullOrWhiteSpace(argCommand)
            || !Shell.NoveCommandManager.TryGetCommand(argCommand, out var cmd))
        {
            Shell.NoveCommandManager.ForEach(x =>
            {
                System.Console.WriteLine("{0}: {1}", x.Name, x.Description);
            });

            System.Console.WriteLine();
            System.Console.WriteLine("Run \"help <command>\" to get more details.");
            return;
        }

        // TODO help command specific
        System.Console.WriteLine("Not implemented");
    }
}
