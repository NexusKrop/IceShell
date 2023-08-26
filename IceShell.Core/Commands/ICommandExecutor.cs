// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

namespace IceShell.Core.Commands;

public interface ICommandExecutor
{
    bool SupportsJump { get; }

    void Jump(string label);
}
