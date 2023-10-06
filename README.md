# IceShell

![GitHub Workflow Status](https://img.shields.io/github/actions/workflow/status/NexusKrop/IceShell/dotnet.yml?style=flat-square&logo=github)
[![GitHub issues](https://img.shields.io/github/issues/NexusKrop/IceShell?style=flat-square)](https://github.com/NexusKrop/IceShell/issues)
[![Trello](https://img.shields.io/badge/-trello-gray?style=flat-square&logo=trello)](https://trello.com/b/eeBRukuy/iceshell)
![GitHub commit activity](https://img.shields.io/github/commit-activity/m/NexusKrop/IceShell?style=flat-square)
[![License](https://img.shields.io/github/license/NexusKrop/IceShell?style=flat-square)](COPYING.txt)

An alternative, cross-platform shell written in C#, and inspired by DOS.

## Usage

See the [IceShell Reference](docs\manual\README.md). Alternatively, type `help` in the IceShell.

## Installation

### Release builds

Before you download a release build, please do note that this project is in early development, and thus developer builds are much
newer than release builds.

Releases can be found at [the releases page](https://github.com/NexusKrop/IceShell/releases).

### Development builds

Development builds can be found at [the Actions page](https://github.com/NexusKrop/IceShell/actions). Click the latest workflow run and scroll down, you'll see a `build-bin` artifact available for download.

- Windows: Extract and run `dotnet iceshell.dll` in the application folder.
- Linux: Extract and run `.\iceshell` in the application folder.

### Build from Source

See [BUILDING](BUILDING.md) file.

## Contributing

For code contributions, pull requests are welcome. For major changes, please open an issue first to discuss what
you like to change.

You can work on issues that have the tag `Status: Open` or `Status: Help wanted`. See [Issues](https://github.com/NexusKrop/IceShell/issues).
You may also report bugs or suggest features through the Issues page.

Check the [Contributing Guide](CONTRIBUTING.md) for more information.

## License

[GPL-3.0-or-later](COPYING.txt)
