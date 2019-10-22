using System;
using System.Diagnostics;
using System.IO;
using ApplicationLogic;
using Fclp;
using ResultRendering;

namespace NHotSpot.Console
{
  public class Program
  {
    //TODO histogram of age (how many files live each number of months)
    //TODO count complexity increase/decrease numbers and increase ratio (how many complexity drops vs complexity increases)
    //TODO add contributors count to hot spot description
    //TODO add percentage of all commits to hot spot description
    //TODO add trend - fastest increasing complexity (not no. of changes)
    static void Main(string[] args)
    {
      //var analysisResult = GitRepoAnalysis.Analyze(@"c:\Users\ftw637\Documents\GitHub\kafka\", "trunk");
      /*
      var analysisConfig = new AnalysisConfig();
      var parser = CreateCliParser(analysisConfig);
      parser.Parse(args);
      */
      Stopwatch sw = new Stopwatch();
      sw.Start();
      //var analysisResult = GitRepoAnalysis.Analyze(@"C:\Users\grzes\Documents\GitHub\kafka", "trunk");
      //var analysisResult = GitRepoAnalysis.Analyze(@"C:\Users\grzes\Documents\GitHub\NSubstitute\", "master");
      //var analysisResult = GitRepoAnalysis.Analyze(@"C:\Users\grzes\Documents\GitHub\nscan\", "master");
      //var analysisResult = GitRepoAnalysis.Analyze(@"c:\Users\ftw637\source\repos\vp-bots\", "master");

      var analysisConfig = new AnalysisConfig()
      {
        //Branch = "trunk",
        Branch = "master",
        MaxCouplingsPerHotSpot = 20,
        MaxHotSpotCount = 100,
        OutputFile = "output.html",
        //RepoPath = @"c:\Users\ftw637\source\repos\vp-bots\"
        //RepoPath = @"C:\Users\ftw637\Documents\GitHub\any"
        //RepoPath = @"C:\Users\ftw637\Documents\GitHub\botbuilder-dotnet\"
        //RepoPath = @"C:\Users\grzes\Documents\GitHub\NSubstitute\",
        //RepoPath = @"C:\Users\grzes\Documents\GitHub\kafka\",
        RepoPath = @"C:\Users\grzes\Documents\GitHub\botbuilder-dotnet",
        MinChangeCount = 2
      };
      var analysisResult = GitRepoAnalysis.Analyze(
        analysisConfig.RepoPath, 
        analysisConfig.Branch, 
        analysisConfig.MinChangeCount, 
        DateTime.Now - TimeSpan.FromDays(366));
      sw.Stop();
      System.Console.WriteLine(sw.ElapsedMilliseconds);
      sw.Reset();
      
      sw.Start();
      var readyDocument = new HtmlAnalysisDocument(analysisConfig)
        .RenderString(analysisResult);
      File.WriteAllText(analysisConfig.OutputFile, readyDocument);
      Browser.Open(analysisConfig.OutputFile);
      sw.Stop();
      System.Console.WriteLine(sw.ElapsedMilliseconds);
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

      p.Setup<string>("output-file")
        .WithDescription("Path to output file")
        .SetDefault("output.html")
        .Callback(outputFilePath => inputArguments.OutputFile = outputFilePath);

      p.SetupHelp("?", "help")
        .Callback(text => System.Console.WriteLine(text));
      return p;
    }
  }
}