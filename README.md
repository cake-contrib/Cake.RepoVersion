# Cake.RepoVersion

A Cake addin that provides automatic versioning using the [repo-version] tool.

| package          | version                            | downloads                    |
| ---------------- | -----------------------------------| -----------------------------|
| Cake.RepoVersion | [![Nuget][current-version]][nuget] | [![Nuget][downloads]][nuget] |

[current-version]: https://img.shields.io/nuget/v/cake.repoversion?style=plastic
[downloads]:       https://img.shields.io/nuget/dt/cake.repoversion?style=plastic
[nuget]:           https://www.nuget.org/packages/cake.repoversion
[repo-version]: https://github.com/kjjuno/repo-version

## Quick Start

```csharp
#addin nuget:?package=Cake.RepoVersion&version=<version>
#addin nuget:?package=Newtonsoft.Json&version=11.0.2

RepositoryVersion version;

Setup(context =>
{
    // Calculate the current version
    version = RepoVersion();

    // Display the current version, or set the build number
    // for the current CI system.
    Information(version.SemVer);
});
```

It is important that you also include the adding for `Newtonsoft.Json` and it must be `11.0.2`

## Contributing

Contributions are very welcome! Head over to the [Contributing](CONTRIBUTING.md) section for more information.

