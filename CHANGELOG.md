# Change-log

This documentation records all visible changes of the project.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added

- Added support for "greedy string values" (last value of a command that can contain spaces without having to be quoted).
- Added support for stream-based mode in `TYPE` command.

### Changed

- Improved support for recognising values that are POSIX paths, instead of integrated command option arguments.

### Fixed

- Fixed `[red][/]` formatting being displayed to the user.

## [v0.1.0-alpha]

- Initial alpha release.

[unreleased]: https://github.com/NexusKrop/IceShell/v0.1.0-alpha...HEAD
[v0.1.0-alpha]: https://github.com/NexusKrop/IceShell/tag/v0.0.1