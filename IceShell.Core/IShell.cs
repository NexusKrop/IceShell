namespace IceShell.Core;

using IceShell.Core.Commands;

public interface IShell : ICommandExecutor
{
    string Prompt { get; set; }

    void Quit();
    int Execute(string line, ICommandExecutor? actualExecutor = null);
}
