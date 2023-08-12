namespace IceShell.Core;

public interface IShell
{
    string Prompt { get; set; }

    void Quit();
}
