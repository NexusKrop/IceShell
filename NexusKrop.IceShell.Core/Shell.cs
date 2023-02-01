namespace NexusKrop.IceShell.Core;

using NexusKrop.IceCube;
using NexusKrop.IceShell.Core;
using NexusKrop.IceShell.Core.CLI;
using NexusKrop.IceShell.Core.Commands;
using NexusKrop.IceShell.Core.FileSystem;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

public class Shell
{
    private readonly CommandParser _parser = new();
    private readonly CommandManager _manager = new();

    private static readonly string WORKINGDIR_EXECUTABLE_DELIMITER = ".\\";

    private bool _exit;

    public void Quit()
    {
        _exit = true;
    }

    public bool ExecuteOnDisk(string fileName, string[]? args)
    {
        var actual = PathSearcher.GetSystemExecutableName(Path.Combine(Environment.CurrentDirectory, fileName));

        if (!File.Exists(actual))
        {
            return false;
        }

        var startInfo = new ProcessStartInfo(actual)
        {
            WorkingDirectory = Environment.CurrentDirectory
        };

        args?.Iterate(x => startInfo.ArgumentList.Add(x));

        Process.Start(startInfo);

        return true;
    }

    public bool ExecuteOnPath(string fileName, string[]? args)
    {
        var actual = PathSearcher.SearchExecutable(fileName);

        if (actual == null)
        {
            return false;
        }

        var startInfo = new ProcessStartInfo(fileName)
        {
            WorkingDirectory = Environment.CurrentDirectory
        };

        args?.Iterate(x => startInfo.ArgumentList.Add(x));

        Process.Start(startInfo);

        return true;
    }

    public void Execute(string input)
    {
        _parser.SetLine(input);

        try
        {
            _parser.ReadCommand(out var command, out var args);

            if (string.IsNullOrWhiteSpace(command))
            {
                ConsoleOutput.PrintShellError(Messages.EmptyCommand);
                return;
            }

            // If starts with "dot limiter" (.\ etc) explicitly execute it in working dir
            if (command.StartsWith(WORKINGDIR_EXECUTABLE_DELIMITER) && !ExecuteOnDisk(command, args))
            {
                ConsoleOutput.PrintShellError(Messages.BadFile);
                return;
            }

            // Get the command from manager
            var cmd = _manager.Get(command);

            // If not a command, check if user specify an executable in PATH
            if (cmd == null)
            {
                // If not, complain
                if (!ExecuteOnPath(command, args))
                {
                    ConsoleOutput.PrintShellError(Messages.BadCommand);
                }

                return;
            }

            // Check if zero arguments, and arguments are specified
            if (cmd.NumArgs == 0 && !(args == null || args.Length == 0))
            {
                ConsoleOutput.PrintShellError(string.Format(Messages.TooManyArguments, cmd.NumArgs));
                return;
            }

            // Check if argument count matches required count.
            // If claimed argument count is below 0, the command will verify themselves.
            if (cmd.NumArgs > 0)
            {
                if (args == null)
                {
                    ConsoleOutput.PrintShellError(string.Format(Messages.NoArguments, cmd.NumArgs));
                    return;
                }

                if (args.Length > cmd.NumArgs)
                {
                    ConsoleOutput.PrintShellError(string.Format(Messages.TooManyArguments, cmd.NumArgs));
                    return;
                }

                if (args.Length < cmd.NumArgs)
                {
                    ConsoleOutput.PrintShellError(string.Format(Messages.TooLessArguments, cmd.NumArgs));
                    return;
                }
            }

            var instance = Activator.CreateInstance(cmd.CommandType);

            if (instance is not ICommand icmd)
            {
                ConsoleOutput.PrintShellError(Messages.BadCommand);
                return;
            }

            icmd.Execute(this, args);
            Console.WriteLine();
        }
        catch (Exception ex)
        {
            ConsoleOutput.PrintShellError(ex.Message);

            Debug.WriteLine(ex.InnerException);
        }
    }

    public int StartInteractive()
    {
        Console.WriteLine();

        while (!_exit)
        {
            Console.Write("{0}> ", Environment.CurrentDirectory);

            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                continue;
            }

            Execute(input);
        }

        return 0;
    }
}
