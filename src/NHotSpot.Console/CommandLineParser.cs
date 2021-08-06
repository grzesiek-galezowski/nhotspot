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
      else if (commandLineParserResult.HelpCalled)
      {
        //bug this shouldn't be an error!
        return commandLineParserResult.ToError<AnalysisConfig, ICommandLineParserResult>();
      }
      return analysisConfig.ToResult<AnalysisConfig, ICommandLineParserResult>();
    }

    private static FluentCommandLineParser CreateCliParser(AnalysisConfig inputArguments)
    {
      var p = new FluentCommandLineParser();

      p.Setup<string>('r', "repository-path")
        .WithDescription("Path to repository root. May be absolute or relative")
        .Callback(repositoryPath => inputArguments.RepoPath = repositoryPath)
        .Required();

      p.Setup<string?>('p', "subfolder-path")
          .WithDescription("Subpath in repository (relative path). " +
                           "Used to shorten analysis time or reduce noise if you are interested only in a subcomponent")
          .Callback(path => inputArguments.Subfolder = 
            path!.ToMaybe().Select(RelativeDirectoryPath.Value))
          .SetDefault(null);
      
      p.Setup<string>('b', "branch")
        .WithDescription("Git branch name. Only committs from on this branch will be analyzed")
        .SetDefault("master")
        .Callback(branch => inputArguments.Branch = branch)
        .Required();

      p.Setup<int>("min-change-count")
        .WithDescription("Minimum change count that makes a file included in the analysis. " +
                         "Used to shorten analysis time when files with low number of changes are irrelevant for the analysis at hand.")
        .Callback(minChangeCount => inputArguments.MinChangeCount = minChangeCount)
        .SetDefault(1)
        .Required();

      p.Setup<int>("max-coupling-per-hospot")
        .WithDescription("Maximum rendered change couplings per hotspot. Used to shorten analysis time.")
        .SetDefault(10)
        .Callback(maxCouplingsPerHotSpot => inputArguments.MaxCouplingsPerHotSpot = maxCouplingsPerHotSpot);

      p.Setup<int>("max-hostpot-count")
        .WithDescription("Maximum count of rendered hotspots. Used to shorten analysis time.")
        .SetDefault(10)
        .Callback(maxHotSpotCount => inputArguments.MaxHotSpotCount = maxHotSpotCount);

      p.Setup<string>("start-date")
        .WithDescription("Start date of the analysis (yyyy-MM-dd). Used to adjust analysis target, e.g. analyze last year")
        .SetDefault(SinceBeginning().ToString("yyyy-MM-dd", CultureInfo.InvariantCulture))
        .Callback(date => inputArguments.StartDate = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture));

      p.Setup<string>('o', "output-file")
        .WithDescription("Path to output HTML file")
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