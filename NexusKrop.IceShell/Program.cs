// Copyright (C) NexusKrop & contributors 2023
// See "COPYING.txt" for licence

using NexusKrop.IceShell.Commands;
using NexusKrop.IceShell.Core;

Shell.ModuleManager.LoadModuleFrom(CommandsModule.Assembly);

return new Shell().RunInteractive();
