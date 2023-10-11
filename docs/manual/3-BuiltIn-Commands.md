# Built-In Commands

> **NOTE**
>
> This section is a work in progress.

This section describes all built-in commands that are available in IceShell and its default modules.

## CALL

_Part of the Batching Module_

## CD

Changes the current working directory. If a directory was not specified, displays the
current working directory.

```bat
cd [directory]
```

### Arguments

- **directory**: _Optional_. The directory to change to.

### Exit Code

Always succeed.

## CLS

Clears the terminal screen.

```cmd
cls
```

### Exit Code

Always succeeds.

## COPY

## DEL

Deletes one or more files.

```cmd
DEL [/P] <files...>
```

### Arguments

- **files**: A list of files to delete.

### Options

- `/P`: If specified, prompts user to confirm before each deletion.

### Exit Code

Returns zero on success and non-zero on failure. For detailed error information, check the exception message.

## DIR

Lists the contents of the current or specified directory.

```cmd
dir [directory] [/d:date-format] [/t:time-format] [/h]
```

### Arguments

- **directory**: _Optional_. The directory to list. If unspecified, lists the current directory.

### Options

- `/d`: Specifies the date format. If unspecified, uses the default format (`YYYY/mm/dd`). 
- `/t`: Specifies the time format. If unspecified, uses the default format (`HH:mm:ss`).
- `/h`: If specified, displays hidden files and directories. On Windows, hidden means being attributed with "hidden" attribute, while on *NIX systems, are ones with name starting with a dot `.`.

### Exit Code

Returns zero on success and non-zero on failure. For detailed error information, check the exception message.

## ECHO

Prints the specified message or redirected output from last command to standard output.

```cmd
echo [message]
```

### Arguments

- **message**: _Optional_. The message to display.

### Exit Code

Always succeeds.

## EXIT

Exits the IceShell. After this command succeeds, IceShell will terminate.

```cmd
exit
```

### Exit Code

Always succeeds.

## GOTO

_Part of the Batching Module_

## HELP

Displays help information.

```cmd
help [command]
```

### Arguments

- **command**: _Optional._ The command to show help message. If unspecified, displays a list of commands and their description.

### Exit Code

Returns zero on success and non-zero on failure. For detailed error information, check the exception message.

## MKDIR

Creates a directory or a tree of directories. The user must be authorised to create the file,
and the directories shall not exist.

```cmd
mkdir <directory>
```

### Arguments

- **directory**: The directory or tree of directories to create.

### Exit Code

Returns `0` if succeeds, non-zero otherwise. Check the message of the error for details.

## MKFILE

Creates a file. The user must be authorised to create the file, and the file specified should not be exist.

```bat
mkfile <file>
```

### Arguments

- **file**: _Required_. The file to create.

### Exit Code

Returns `0` if succeeds, non-zero otherwise. Check the message of the error for details.

## MOVE

Move or rename one or more files.

```cmd
move [/F] <files...>
```

### Arguments

- **files**: _Requried_. The files to move.

### Exit Code

Returns `0` if succeeds, non-zero otherwise. Check the message of the error for details.

## PROMPT

## REM

Does nothing.

```REM
rem [any argument...]
```

### Exit Code

Always succeed.

## START

## TYPE

## VER

Displays the IceShell version along with some Operating System information.

```bat
ver
```

### Exit Code

Always succeed.

---

Copyright (C) 2023 IceShell contributors.

Permission is granted to copy, distribute and/or modify this document under the terms of the GNU Free Documentation License, Version 1.3 or any later version published by the Free Software Foundation; with no Invariant Sections, no Front-Cover Texts, and no Back-Cover Texts.

A copy of the license is included in the file named "LICENSE-DOCS", in the `docs/manual` directory of the project repository.
