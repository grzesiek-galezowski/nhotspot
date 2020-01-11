using System;
using System.IO;
using System.Reflection;
using System.Threading.Channels;
using static AtmaFileSystem.AtmaFileSystemPaths;
using static Bullseye.Targets;
using static SimpleExec.Command;

namespace Build
{
  public class Script
  {
    static void Main(string[] args)
    {
      var repositoryRoot = AbsoluteDirectoryPath(Git.CurrentRepositoryPath());
      var slnPath = repositoryRoot + DirectoryName("src") + DirectoryName("GitAttempt2");
      var consoleAppPath = slnPath + DirectoryName("GitAttempt2") + FileName("NHotSpot.Console.csproj");

      Target("clean", () => Run("rm", "-r ./output"));
      Target("build", () => Run("dotnet", $"build {slnPath}"));
      Target("test", DependsOn("build"), () => Run("dotnet", $"test {slnPath}"));
      Target("pack", DependsOn("test"), () => Run("dotnet", 
        $"pack {consoleAppPath} " + //bug how to pack everything?
        "-p:VersionPrefix=1.0.3 " + 
        "-o ./output "
      ));
      Target("publish", DependsOn("clean"), () => Run("dotnet", "publish -o ./output"));
      Target("default", DependsOn("pack"));

      RunTargetsAndExit(args);
    }
  }
}
