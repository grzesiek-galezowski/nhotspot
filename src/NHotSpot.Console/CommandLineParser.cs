using System;
using System.Globalization;
using AtmaFileSystem;
using Fclp;
using Functional.Either;
using Functional.Maybe;
using NHotSpot.ApplicationLogic;

namespace NHotSpot.Console
{
  public static class CommandLineParser
  {
    public static Either<AnalysisConfig, ICommandLineParserResult> Parse(string[] args)
    {
      var analysisConfig = new AnalysisConfig();
      var parser = CreateCliParser(analysisConfig);
      var commandLineParserResult = parser.Parse(args);
      if (commandLineParserResult.HasErrors)
      {
        return commandLineParserResult.ToError<AnalysisConfig, ICommandLineParserResult>();
      }
      return analysisConfig.ToResult<AnalysisConfig, ICommandLineParserResult>();
    }

    private static FluentCommandLineParser CreateCliParser(AnalysisConfig inputArguments)
    {
      var p = new FluentCommandLineParser();

      p.Setup<string>('r', "repository-path")
        .WithDescription("Path to repository root")
        .Callback(repositoryPath => inputArguments.RepoPath = repositoryPath)
        .Required();

      p.Setup<string?>('p', "subfolder-path")
          .WithDescription("Subpath in repository (relative path)")
          .Callback(path => inputArguments.Subfolder = 
            path!.ToMaybe().Select(RelativeDirectoryPath.Value))
          .SetDefault(null);
      
      p.Setup<string>('b', "branch")
        .WithDescription("branch name")
        .SetDefault("master")
        .Callback(branch => inputArguments.Branch = branch)
        .Required();

      p.Setup<int>("min-change-count")
        .WithDescription("Minimum change count that makes a file count")
        .Callback(minChangeCount => inputArguments.MinChangeCount = minChangeCount)
        .SetDefault(1)
        .Required();

      p.Setup<int>("max-coupling-per-hospot")
        .WithDescription("Maximum rendered change couplings per hotspot")
        .SetDefault(10)
        .Callback(maxCouplingsPerHotSpot => inputArguments.MaxCouplingsPerHotSpot = maxCouplingsPerHotSpot);

      p.Setup<int>("max-hostpot-count")
        .WithDescription("Maximum count of rendered hotspots")
        .SetDefault(10)
        .Callback(maxHotSpotCount => inputArguments.MaxHotSpotCount = maxHotSpotCount);

      p.Setup<string>("start-date")
        .WithDescription("Start date of the analysis (yyyy-MM-dd)")
        .SetDefault(SinceBeginning().ToString("yyyy-MM-dd", CultureInfo.InvariantCulture))
        .Callback(date => inputArguments.StartDate = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture));

      p.Setup<string>('o', "output-file")
        .WithDescription("Path to output file")
        .SetDefault("output.html")
        .Callback(outputFilePath => inputArguments.OutputFile = outputFilePath);

      p.SetupHelp("?", "help")
        .Callback(text => System.Console.WriteLine(text));
      return p;
    }

    private static DateTime SinceBeginning()
    {
        return DateTime.MinValue + TimeSpan.FromDays(300);
    }
  }
}