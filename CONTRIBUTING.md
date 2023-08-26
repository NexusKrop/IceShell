# Contributing Guidelines

Thank you for taking time to improve this very project, whether by
reporting bugs, suggesting features, or contributing code.

In this guide you will know about:

- Ways to contribute
- How do you effectively contribute

## Ways to contribute

There are plenty of ways to contribute, either by writing code to improve this
project, or by other ways (some of them did not even need you to write a single
line of code!).

### Issues

Anyone can report bugs, make suggestions or request new features via the Issues
section of the repository. [Here's an link to it.](https://github.com/NexusKrop/IceShell/issues)

#### Bugs

If you are reporting bugs or other sorts of issues of the project, the report
should include the following information:

- The description of the bug
- Detailed information how to reproduce the bug
- What platform are you experiencing the issue with (OS version, .NET version,
  etc.)
- Which version of IceShell are you using (obtain via running `ver`, 
  or on the CI or release page you obtained the download from), make sure to
  include the section after `+` if version was obtained through `ver` command

### Contributing Code

Contributing code is the most direct form of contribution to this project.

If you want to contribute code but can't find anything to work on, try browsing
our [Trello board](https://trello.com/b/eeBRukuy/iceshell) and the Issues
section for issues that is not currently being worked on.

#### Building the Project

This project does not need specific steps to build. However, if you need a how
to build information:

- Clone the repository.
- Navigate to the repository directory and open your terminal.
- Make sure .NET SDK is installed.
- Run `dotnet build`.

#### How to Contribute Code

If you need to make major changes, please do open an issue to discuss it first.

For code contributions, you should use Pull Request.

- First, _fork_ this repository, and name it whatever you like.
- Make your changes.
- Add/update tests as appropriate.
- Run unit tests.
- Create Pull Request from your repository. This is usually done by clicking
  `Contribute` in your repository's homepage or via the Pull Requests section.

Anyone can contributing code to this project via the Pull Requests feature of
GitHub. But, please do stick to the following guidelines if you are
contributing code to keep this project tidy:

- Make sure all new dependencies you have added conforms to the
  [Dependency Guidelines](https://github.com/NexusKrop/.github/blob/main/docs/dependencies.md).
- Make sure all existing unit tests pass, and new features are tested promptly.
- Follow the code styles of the project.

