# 0. Introduction

IceShell is an experimental shell interpreter written in C#. Its syntax and format is inspired by Windows Command Prompt and MS-DOS.

> [!NOTE]
> This documentation is written in October 2023. Updates after the date may render this documentation outdated. Please check [the CHANGELOG](../../CHANGELOG.md) for changes.

This shell interpreter is mostly a hobby project, and should not be used for remote or advanced system administration. For more information, see this [page](Appendix-A-Security.md).

## Getting Started

### Installation

You can download IceShell via any of the methods below:

#### GitHub Releases

You can download release builds from [GitHub releases](https://github.com/NexusKrop/IceShell/releases). These builds are usually provided for Windows and GNU/Linux.

macOS users can use `dotnet iceshell.dll` instead of `iceshell.exe` to start the program.

#### GitHub Actions Artefacts

You can download unstable builds via [CI Artefacts](https://github.com/NexusKrop/IceShell/actions). An unstable build is automatically created for each code change, and thus
are not verified and can contain many bugs. Use them only for testing.

Only builds with GNU/Linux launcher are provided, but you can use `dotnet iceshell.dll` to start IceShell on other systems.

### Usage

#### Hello World Example

Type the following in the IceShell command prompt:

```bat
echo Hello World!
```

And the IceShell should output:

```plain
Hello World!
```

For more information about the shell, please check the following pages:

- [Syntax](2-Syntax.md)
- [Built-in commands](3-BuiltIn-Commands.md)

---

Copyright (C) 2023 IceShell contributors.

Permission is granted to copy, distribute and/or modify this document under the terms of the GNU Free Documentation License, Version 1.3 or any later version published by the Free Software Foundation; with no Invariant Sections, no Front-Cover Texts, and no Back-Cover Texts.

A copy of the license is included in the file named "LICENSE-DOCS", in the `docs/manual` directory of the project repository.
