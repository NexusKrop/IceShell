using System.CommandLine;
using System.CommandLine.Help;
using System.CommandLine.Invocation;
using NexusKrop.IceCube;

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
            || !Shell.NoveCommandManager.TryGetCommand(argCommand, out var cmd)
            || cmd == null)
        {
            Shell.NoveCommandManager.ForEach(x =>
            {
                System.Console.WriteLine("{0}: {1}", x.Name, x.Description);
            });

            System.Console.WriteLine();
            System.Console.WriteLine("Run \"help <command>\" to get more details.");
            return;
        }

        // See https://github.com/dotnet/command-line-api/blob/main/src/System.CommandLine/Help/HelpOptionAction.cs
        var builder = new HelpBuilder(LocalizationResources.Instance, Console.IsOutputRedirected ? int.MaxValue : Console.WindowWidth);

        var parseResult = cmd.Parse();

        var helpContext = new HelpContext(builder,
                                  cmd,
                                  Console.Out);
        builder.Write(helpContext);
    }
}
