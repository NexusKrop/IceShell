# Built-In Commands

> **NOTE**
>
> This section is a work in progress.

This section describes all built-in commands that are available in IceShell.

## Shell Operation Commands

### CD

Changes the current working directory. If a directory was not specified, displays the
current working directory.

```bat
cd [directory]
```

#### Arguments

- **directory**: _Optional_. The directory to change to.

#### Exit Code

Always succeed.

### ECHO

Prints the specified message or redirected output from last command to standard output.

```cmd
echo [message]
```

#### Arguments

- **message**: _Optional_. The message to display.

#### Exit Code

Always succeeds.

### EXIT

Exits the IceShell. After this command succeeds, IceShell will terminate.

```cmd
exit
```

#### Exit Code

Always succeeds.

### VER

Displays the IceShell version along with some Operating System information.

```bat
ver
```

#### Exit Code

Always succeed.

## File Operation Commands

### MKDIR

Creates a directory or a tree of directories. The user must be authorised to create the file,
and the directories shall not exist.

```cmd
mkdir <directory>
```

#### Arguments

- **directory**: The directory or tree of directories to create.

#### Exit Code

Returns `0` if succeeds, non-zero otherwise. Check the message of the error for details.

### MKFILE

Creates a file. The user must be authorised to create the file, and the file specified should not be exist.

```bat
mkfile <file>
```

#### Arguments

- **file**: _Required_. The file to create.

#### Exit Code

Returns `0` if succeeds, non-zero otherwise. Check the message of the error for details.

---

Copyright (C) 2023 IceShell contributors.

Permission is granted to copy, distribute and/or modify this document under the terms of the GNU Free Documentation License, Version 1.3 or any later version published by the Free Software Foundation; with no Invariant Sections, no Front-Cover Texts, and no Back-Cover Texts.

A copy of the license is included in the file named "LICENSE-DOCS", in the `docs/manual` directory of the project repository.
