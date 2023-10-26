# Change-log

This documentation records all visible changes of the project.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added

#### Syntax

- Added support for single AND symbol operator (`&`) which executes the next command regardless of the result of the last command.

#### API

- Added support for `SByte`, `Byte`, `Int16`, `Int32` and `Int64` arguments.

#### Shell

- Added support for extended prompt format.

### Changed

#### Commands

- `PROMPT` command will check the syntax of the prompt template.

## [v0.2.0-alpha]

### Added

#### Commands

- Introduced `IF` command to check for certain conditions and circumstances.
- Introduced `SET` command to query and set environment variables.
- Added support for error codes for internal commands.

#### Syntax

- Added support for "greedy string values" (last value of a command that can contain spaces without having to be quoted).
- Added a new operator (`&&`) that only executes the command to its right if the command to its left returns exit code 0.

#### Shell

- Added experimental support for redirecting output from and to commands.
- Added an option in the configuration to display shell interpreter version info on start-up.
- Added support for `CTRL` + `C` interruption.
  - When a process launched from the shell is currently running, IceShell either sends `SIGTERM` on Linux (if configured to do so) or kills the process in other cases.
  - Otherwise, the shell exits immediately.
- Added support for saving and restoring command history.
- Added `SIGTERM` handling support for GNU/Linux.

#### API

- Added API support for expanding environment variables.

### Changed

#### Commands

- Added support for stream-based mode in `TYPE` command.
- Added support for `DIR` command to emit machine readable format.
- `DIR` command will now fail if a directory was specified, and the specified directory does not exist.

#### Syntax

- Improved support for recognising values that are POSIX paths, instead of integrated command option arguments.
- End of options statement (`--`) can no longer be quoted.
- Escaping is now disallowed outside quoted strings.

#### Shell

- Command history is now to limited 100 items. 

#### API

- `ICommand` interface is now replaced with `IShellCommand` interface.

### Fixed

- Addressed an issue resulted in Spectre.Console mark-ups no longer rendered properly in error reporting.
- Addressed an issue resulted in external commands no longer being recognised.
- Addressed an issue where `-255` is always returned (instead of the exit code of the command executed) when the `iceshell` executable is used to execute command rather than launching the interactive shell.
- Addressed an issue where `HELP` command fails in certain conditions due to markup syntaxes in command aliases and descriptions being recognised.

## [v0.1.0-alpha]

- Initial alpha release.

[unreleased]: https://github.com/NexusKrop/IceShell/v0.2.0-alpha...HEAD
[v0.2.0-alpha]: https://github.com/NexusKrop/IceShell/tag/v0.2.0
[v0.1.0-alpha]: https://github.com/NexusKrop/IceShell/tag/v0.1.0