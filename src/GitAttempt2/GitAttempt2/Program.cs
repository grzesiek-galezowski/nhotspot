﻿using System.Diagnostics;
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

      var analysisConfig = new AnalysisConfig();
      var parser = CreateCliParser(analysisConfig);
      parser.Parse(args);

      Stopwatch sw = new Stopwatch();
      sw.Start();
      //var analysisResult = GitRepoAnalysis.Analyze(@"C:\Users\grzes\Documents\GitHub\kafka", "trunk");
      //var analysisResult = GitRepoAnalysis.Analyze(@"C:\Users\grzes\Documents\GitHub\NSubstitute\", "master");
      //var analysisResult = GitRepoAnalysis.Analyze(@"C:\Users\grzes\Documents\GitHub\nscan\", "master");
      //var analysisResult = GitRepoAnalysis.Analyze(@"c:\Users\ftw637\source\repos\vp-bots\", "master");

      var analysisResult = GitRepoAnalysis.Analyze(analysisConfig.RepoPath, analysisConfig.Branch);
      sw.Stop();
      System.Console.WriteLine(sw.ElapsedMilliseconds);
      sw.Reset();
      sw.Start();
      new HtmlChartOutput(analysisConfig).Show(analysisResult);
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