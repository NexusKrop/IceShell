namespace IceShell.Core.Commands;

public interface ICommandExecutor
{
    bool SupportsJump { get; }

    void Jump(string label);
}
