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
      var slnPath = repositoryRoot + DirectoryName("src");
      var consoleAppPath = slnPath + DirectoryName("NHotSpot.Console");
      var outputPath = repositoryRoot + DirectoryName("output");
      const string version = "0.4.0";
      
      Target("clean", () =>
      {
        Run("rm", $"-r {outputPath}");
      });
      Target("build", () =>
      {
        Run("dotnet", 
          Args(
            $"build {consoleAppPath} ",
            $"-p:VersionPrefix={version} ",
            "-c Release ",
            $"-o {outputPath} "
            ));
      });
      Target("test", DependsOn("build"), () =>
      {
        Run("dotnet", $"test {slnPath}");
      });
      Target("pack", DependsOn("test"), () =>
      {
        Run("dotnet",
          Args(
            $"pack {consoleAppPath}", 
            "--include-symbols", 
            "-p:SymbolPackageFormat=snupkg", 
            $"-p:VersionPrefix={version} ", 
            "-c Release", 
            $"-o {outputPath} "));
      });

      Target("default", DependsOn("pack"));

      RunTargetsAndExit(args);
    }

    private static string Args(params string[] strings)
    {
      return string.Join(' ', strings);
    }
  }
}
