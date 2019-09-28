using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Cake.Core;
using Cake.Core.Annotations;
using Newtonsoft.Json;

namespace Cake.RepoVersion
{
    /// <summary>
    /// Cake Method Aliases for repo-version
    /// </summary>
	public static class MethodAliases
    {
        /// <summary>
        /// Calculates the version of the current repository.
        /// </summary>
        /// <param name="context">The <see cref="ICakeContext" />.</param>
        /// <returns>A <see cref="RepositoryVersion" /> populated with the current repository version.</returns>
        [CakeMethodAlias]
        [CakeNamespaceImport("Cake.RepoVersion")]
        public static RepositoryVersion RepoVersion(this ICakeContext context)
        {
            EnsureRepoVersionIsInstalled();
            var path = Path.Combine(GetToolPath(), "repo-version");

            var json = Exec(path, "-o json");

            return JsonConvert.DeserializeObject<RepositoryVersion>(json);
        }

        private static void EnsureRepoVersionIsInstalled()
        {
            var version = "0.2.10.2";
            var toolPath = GetToolPath();

            // Get the latest version of repo-version that matches our major.minor
            var text = Exec("dotnet", $"tool install --tool-path {toolPath} repo-version --version {version}");
            Console.WriteLine(text);
        }

        private static string GetToolPath()
        {
            var location = Assembly.GetExecutingAssembly().Location;
            var dir = Path.GetDirectoryName(location);
            return Path.Combine(dir, "dotnet-tools");
        }

        private static string Exec(string filename, string arguments)
        {
            var info = new ProcessStartInfo
            {
                FileName = filename,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };
            var proc = Process.Start(info);
            proc.WaitForExit();

            return proc.StandardOutput.ReadToEnd();
        }
    }
}
