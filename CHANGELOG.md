# Change-log

This documentation records all visible changes of the project.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added

- Added support for "greedy string values" (last value of a command that can contain spaces without having to be quoted).
- Added support for stream-based mode in `TYPE` command.
- Added experimental support for redirecting:
  - The output of an internal command to another internal command;
  - The output of an internal command to an external command;
  - The output of an external command to an internal command;
  - The output of an external command to another external command.
- Added support for `DIR` command to emit machine readable format.
- Added an option in the configuration to display shell interpreter version info on start-up.
- Added API support for expanding environment variables.
- Added support for `CTRL` + `C` interruption.
  - When a process launched from the shell is currently running, IceShell either sends `SIGTERM` on Linux (if configured to do so) or kills the process in other cases.
  - Otherwise, the shell exits immediately.
- Added support for saving and restoring command history.
- Added `SIGTERM` handling support for GNU/Linux.
- 

### Changed

- Improved support for recognising values that are POSIX paths, instead of integrated command option arguments.
- End of options statement (`--`) can no longer be quoted.
- Escaping is now disallowed outside quoted strings.
- `DIR` command will now fail if a directory was specified, and the specified directory does not exist.
- Command history is now limited 100 items. 

### Fixed

- Addressed an issue resulted in Spectre.Console mark-ups no longer rendered properly in error reporting.
- Addressed an issue resulted in external commands no longer working.
- Addressed an issue where `-255` is always returned (instead of the exit code of the command executed) when the `iceshell` executable is used to execute command rather than launching the interactive shell.

## [v0.1.0-alpha]

- Initial alpha release.

[unreleased]: https://github.com/NexusKrop/IceShell/v0.1.0-alpha...HEAD
[v0.1.0-alpha]: https://github.com/NexusKrop/IceShell/tag/v0.0.1