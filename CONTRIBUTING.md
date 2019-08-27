# Contributing

All contributions are welcome, no matter how small. If you are new to github and are looking
for an easy way to contribute, documentation is always appreciated.

Another great way to contribute is simply to write up issues. If you are aware of any defects,
or would really like to see a new feature, please create an issue.

## Development Requirements

1. .NET Core SDK 2.1 or greater
2. mono (MacOS and Linux only)
3. powershell

Or, click [here](https://gitpod.io#https://github.com/cake-contrib/Cake.RepoVersion) to open a ready to go [Gitpod] environment

[Gitpod]: https://gitpod.io

## How to build

This project uses a cake build script (of course!). From powershell run the following command. This will work on
powershell core, so it should work on Windows, Linux, and MacOS

```bash
.\build.ps1
```

optionally you can provide a specific target


```bash
.\build.ps1 -Target <target>
```

Available Targets:

| Target    | Description                                |
| --------- | ------------------------------------------ |
| Pack      | (Default) Creates NuGet packages           |
| Publish   | Publishes nuget package to nuget.org       |

