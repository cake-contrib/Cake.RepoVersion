#addin nuget:?package=Cake.Json&version=4.0.0
#addin nuget:?package=Cake.RepoVersion&version=0.2.7.1
#addin nuget:?package=Newtonsoft.Json&version=11.0.2

var target = Argument("target", "Pack");
RepositoryVersion version;

Setup(context =>
{
    version = RepoVersion();

    Information($"Version: {version.SemVer}");

    if (BuildSystem.IsRunningOnAppVeyor)
    {
        Information(EnvironmentVariable("APPVEYOR_PULL_REQUEST_NUMBER"));
        Information(EnvironmentVariable("APPVEYOR_PULL_REQUEST_TITLE"));
        Information(EnvironmentVariable("APPVEYOR_PULL_REQUEST_HEAD_REPO_NAME"));
        Information(EnvironmentVariable("APPVEYOR_PULL_REQUEST_HEAD_REPO_BRANCH"));

        AppVeyor.UpdateBuildVersion(version.SemVer);
    }

});

Task("Pack")
    .Does(() =>
    {

        DotNetCorePack(".", new DotNetCorePackSettings
            {
                Configuration = "Release",
                EnvironmentVariables = new Dictionary<string, string>
                {
                    ["VERSION"] = version.SemVer
                }
            });

    });

Task("Publish")
    .IsDependentOn("Pack")
    .Does(() =>
    {
        IEnumerable<string> redirectedStandardOutput;
        var exitCodeWithArgument = StartProcess("git", new ProcessSettings 
            {
                Arguments = "rev-parse --abbrev-ref HEAD",
                RedirectStandardOutput = true
            },
            out redirectedStandardOutput);

        var branch = redirectedStandardOutput.FirstOrDefault();

        Information($"Current Branch: {branch}");

        var apiKey = EnvironmentVariable("NUGET_API_KEY");

        if (string.IsNullOrEmpty(apiKey))
        {
            throw new Exception("No value for NUGET_API_KEY");
        }

         var settings = new DotNetCoreNuGetPushSettings
         {
             Source = "https://www.nuget.org/api/v2/package",
             ApiKey = apiKey
         };

         DotNetCoreNuGetPush($"nupkg/Cake.RepoVersion.{version.SemVer}.nupkg", settings);
    });

Task("AppVeyor")
    .IsDependentOn("Pack");

RunTarget(target);
