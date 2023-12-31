<!-- If you forgot the ID it may not render correctly -->

<div id="iceshell-readme-centre-logo" align="center">
<img src="assets/logo_with_bg.png" />
</div>
<h1 style="text-align: center;">IceShell</h1>

<div id="iceshell-readme-centre-badges" align="center">

![GitHub Workflow Status](https://img.shields.io/github/actions/workflow/status/NexusKrop/IceShell/dotnet.yml?style=flat-square&logo=github)
[![GitHub issues](https://img.shields.io/github/issues/NexusKrop/IceShell?style=flat-square)](https://github.com/NexusKrop/IceShell/issues)
[![Trello](https://img.shields.io/badge/-trello-gray?style=flat-square&logo=trello)](https://trello.com/b/eeBRukuy/iceshell)
![GitHub commit activity](https://img.shields.io/github/commit-activity/m/NexusKrop/IceShell?style=flat-square)
[![License](https://img.shields.io/github/license/NexusKrop/IceShell?style=flat-square)](COPYING.txt)

</div>

<div id="iceshell-readme-centre-description" align="center">

A cross-platform shell interpreter written in C#, and is inspired by Windows Command Prompt and
MS-DOS `command.com`.

</div>

## Usage

See the [IceShell Reference](docs/manual/README.md). Alternatively, type `help` in the IceShell.

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

We welcome contributions! See the [Contributing Guide](CONTRIBUTING.md) for more information.

## License

[GPL-3.0-or-later](COPYING.txt)
