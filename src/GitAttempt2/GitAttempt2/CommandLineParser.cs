using ApplicationLogic;
using Fclp;

namespace NHotSpot.Console
{
  public static class CommandLineParser
  {
    public static AnalysisConfig Parse(string[] args)
    {
      var analysisConfig = new AnalysisConfig();
      var parser = CreateCliParser(analysisConfig);
      parser.Parse(args);
      return analysisConfig;
    }

    private static FluentCommandLineParser CreateCliParser(AnalysisConfig inputArguments)
    {
      var p = new FluentCommandLineParser();

      p.Setup<string>('r', "repository-path")
        .WithDescription("Path to repository root")
        .Callback(repositoryPath => inputArguments.RepoPath = repositoryPath)
        .Required();
      
      p.Setup<string>('b', "branch")
        .WithDescription("branch name")
        .Callback(branch => inputArguments.Branch = branch)
        .Required();

      p.Setup<int>("min-change-count")
        .WithDescription("Minimum change count that makes a file count")
        .Callback(minChangeCount => inputArguments.MinChangeCount = minChangeCount)
        .Required();

      p.Setup<int>("max-coupling-per-hospot")
        .WithDescription("Maximum rendered change couplings per hotspot")
        .SetDefault(20)
        .Callback(maxCouplingsPerHotSpot => inputArguments.MaxCouplingsPerHotSpot = maxCouplingsPerHotSpot);

      p.Setup<int>("max-hostpot-count")
        .WithDescription("Maximum count of rendered hotspots")
        .SetDefault(100)
        .Callback(maxHotSpotCount => inputArguments.MaxHotSpotCount = maxHotSpotCount);

      p.Setup<string>('o', "output-file")
        .WithDescription("Path to output file")
        .SetDefault("output.html")
        .Callback(outputFilePath => inputArguments.OutputFile = outputFilePath);

      p.SetupHelp("?", "help")
        .Callback(text => System.Console.WriteLine(text));
      return p;
    }
  }
}