# Building Guidelines

IceShell depends on [IceCube](https://github.com/NexusKrop/IceCube) which is hosted on our MyGet feed.

## With .NET CLI

Building IceShell with CLI requires .NET 6.0 SDK or later.

### Add MyGet source

Add the [MyGet](https://www.myget.org/feed/Details/nexuskrop) feed by using
the following command:

```sh
dotnet nuget add source https://www.myget.org/F/nexuskrop/api/v3/index.json --name NexusKrop MyGet
```

### Build

- Navigate to your solution folder.
- Run `dotnet build`.
- Binaries should be in `NexusKrop.IceShell\bin`.

## With Visual Studio

It is recommended that you execute the following command in IDE Terminal:

```sh
dotnet nuget add source https://www.myget.org/F/nexuskrop/api/v3/index.json --name NexusKrop MyGet
```

And then build the project.