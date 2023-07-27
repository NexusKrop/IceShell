# Bundled Commands

Like the `command.com` of MS-DOS, IceShell comes with bundled management commands.

## Utility Commands

### help

Usage: `help [command]`

This command at this stage can print a basic help information for commands.

### exit

Usage: `exit`

Exits the IceShell.

### cd

Usage: `cd [dir]`

If `dir` is not specified, prints the current working directory to the console; otherwise, changes the current working directory.

### ver

Usage: `ver`

Displays the IceShell and system related information.

## File Operation Commands

### copy

Usage: `copy <source> <destination> [/f]`

Copies a file from the `source` to `destination`. If `/f` is specified, overrides the destination forcefully (overwrite existing files, etc).

### mkfile

Usage: `mkfile <name>`

Creates an empty file.

### rm
