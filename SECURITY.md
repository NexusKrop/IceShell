# Security Guidelines and Information

Unless put into a properly restricted environment, IceShell is not secure enough to execute untrusted commands or process
any untrusted information (it is not designed to execute untrusted code outside trusted environment). Issues caused by executing untrusted code
in an IceShell instance running in unrestricted environments are security issues of your side, not us.

However, issues resulting in untrusted code _escaping from_ restricted environments that are not due to your configuration failure or some other programs
should be reported. Please make sure that you are confident that the issue is not caused by configuration issues from your side.

You should refrain from reporting security issues via public GitHub issues. Security issues should be reported directly to maintainers,
which are listed in the [Code of Conduct](CODE_OF_CONDUCT.md) documentation. Issues will be disclosed once the issue have been addressed and a reasonable time have been given to allow
users to update.
