using Build;
using static AtmaFileSystem.AtmaFileSystemPaths;
using static Bullseye.Targets;
using static SimpleExec.Command;

const string consoleAppName = "NHotSpot.Console";
const string version = "0.7.1";
var repositoryRoot = AbsoluteDirectoryPath(Git.CurrentRepositoryPath());
var slnPath = repositoryRoot + DirectoryName("src");
var consoleAppPath = slnPath + DirectoryName(consoleAppName);
var outputPath = repositoryRoot + DirectoryName("output");

Target("clean", () =>
{
  Run("rm", $"-r {outputPath}");
});

Target("build", () =>
  DotNet(
    $"build {consoleAppPath} ",
    $"-p:VersionPrefix={version} ",
    "-c Release ",
    $"-o {outputPath} "
  )
);

Target("test", DependsOn("build"), () =>
  DotNet($"test {slnPath}"));

Target("pack", DependsOn("test"), () =>
  DotNet(
    $"pack {consoleAppPath}",
    "--include-symbols",
    "-p:SymbolPackageFormat=snupkg",
    $"-p:VersionPrefix={version} ",
    "-c Release",
    $"-o {outputPath} ")
);

Target("push", DependsOn("pack"), () =>
{
  var absoluteFilePath = outputPath.AddFileName($"TddXt.{consoleAppName}.{version}.nupkg");
  Run("dotnet", Args($"nuget push {absoluteFilePath}", "--source https://api.nuget.org/v3/index.json"));
});

Target("default", DependsOn("pack"));

RunTargetsAndExit(args);


//////////////////////
// HELPER FUNCTIONS
//////////////////////

void DotNet(params string[] args)
{
  Run("dotnet", Args(args));
}

string Args(params string[] strings)
{
  return string.Join(' ', strings);
}
