namespace IceShell.Core.CLI;

public interface ITerminal
{
    TextWriter StandardOutput { get; }
    ConsoleKeyInfo ReadKey();
}
