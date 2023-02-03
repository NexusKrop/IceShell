namespace NexusKrop.IceShell.Core;

using NexusKrop.IceCube;
using NexusKrop.IceShell.Core;
using NexusKrop.IceShell.Core.CLI;
using NexusKrop.IceShell.Core.Commands;
using NexusKrop.IceShell.Core.Commands.Complex;
using NexusKrop.IceShell.Core.Completion;
using NexusKrop.IceShell.Core.Completion.Cache;
using NexusKrop.IceShell.Core.Exceptions;
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

    private static readonly DirCache DIR_CACHE = new(Environment.CurrentDirectory);
    private static readonly string WORKINGDIR_EXECUTABLE_DELIMITER = ".\\";

    public static bool ExitShell { get; set; }

    public static void ChangeDirectory(string target)
    {
        Directory.SetCurrentDirectory(target);
        DIR_CACHE.UpdateDirectory(target);
    }

    public static void Quit()
    {
        ExitShell = true;
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

        if (!FileUtil.IsExecutable(actual))
        {
            throw new CommandFormatException(Messages.FileNotExecutable);
        }

        Process.Start(startInfo)?.WaitForExit();

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

        Process.Start(startInfo)?.WaitForExit();

        return true;
    }

    public void Execute(string input)
    {
        _parser.SetLine(input);

        try
        {
            var command = _parser.ReadString();
            string[]? args = null;

            if (string.IsNullOrWhiteSpace(command))
            {
                ConsoleOutput.PrintShellError(Messages.EmptyCommand);
                return;
            }

            // If starts with "dot limiter" (.\ etc) explicitly execute it in working dir
            if (command.StartsWith(WORKINGDIR_EXECUTABLE_DELIMITER))
            {
                _parser.ReadArgs(out args);

                if (!ExecuteOnDisk(command, args))
                {
                    ConsoleOutput.PrintShellError(Messages.BadFile);
                }

                return;
            }

            var type = _manager.GetComplex(command);

            // If no such complex command either
            if (type == null)
            {
                // Reads the arguments out
                _parser.ReadArgs(out args);

                // Executes them on path
                if (!ExecuteOnPath(command, args))
                {
                    // If no such path file, say bad command
                    ConsoleOutput.PrintShellError(Messages.BadCommand);
                }

                return;
            }

            var instance = Activator.CreateInstance(type);

            if (instance is IComplexCommand ixcmd)
            {
                var arg = new ComplexArgument(_parser);

                ixcmd.Define(arg);
                ixcmd.Execute(arg.Parse());
                Console.WriteLine();
            }
            else
            {
                ConsoleOutput.PrintShellError(Messages.BadCommand);
            }
        }
        catch (CommandFormatException ex)
        {
            ConsoleOutput.PrintShellError(ex.Message);
        }
        catch (Exception ex)
        {
            ConsoleOutput.PrintShellError(ex.ToString());
        }
    }

    public int StartInteractive()
    {
        Console.WriteLine();
        ReadLine.HistoryEnabled = true;
        ReadLine.AutoCompletionHandler = new ShellCompletionHandler(_manager, DIR_CACHE);

        while (!ExitShell)
        {
            var input = ReadLine.Read(string.Format("{0}> ", Environment.CurrentDirectory));

            if (string.IsNullOrWhiteSpace(input))
            {
                continue;
            }

            Execute(input);
        }

        return 0;
    }
}
