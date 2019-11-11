using System;
using System.Diagnostics;
using System.IO;
using ApplicationLogic;
using Fclp;
using GitAnalysis;
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

      var analysisConfig = new AnalysisConfig()
      {
        //Branch = "trunk",
        Branch = "master",
        MaxCouplingsPerHotSpot = 20,
        MaxHotSpotCount = 100,
        OutputFile = "output.html",
        //RepoPath = @"c:\Users\ftw637\source\repos\vp-bots\",
        //RepoPath = @"C:\Users\ftw637\Documents\GitHub\any",
        //RepoPath = @"C:\Users\grzes\Documents\GitHub\any",
        RepoPath = @"C:\Users\grzes\Documents\GitHub\nscan",
        //RepoPath = @"C:\Users\grzes\Documents\GitHub\AutoFixtureGenerator\",
        //RepoPath = @"C:\Users\grzes\Documents\GitHub\TrainingExamples\",
        //RepoPath = @"C:\Users\grzes\Documents\GitHub\tdd-ebook\",
        //RepoPath = @"C:\Users\grzes\Documents\GitHub\AutoFixture\",
        //RepoPath = @"C:\Users\grzes\Documents\GitHub\simple-nlp\",
        //RepoPath = @"C:\Users\grzes\Documents\GitHub\Functional.Maybe\",
        //RepoPath = @"C:\Users\grzes\Documents\GitHub\nodatime\",
        //RepoPath = @"C:\Users\ftw637\Documents\GitHub\botbuilder-dotnet\",
        //RepoPath = @"C:\Users\grzes\Documents\GitHub\NSubstitute\",
        //RepoPath = @"C:\Users\grzes\Documents\GitHub\kafka\",
        //RepoPath = @"C:\Users\grzes\Documents\GitHub\botbuilder-dotnet",
        //RepoPath = @"C:\Users\grzes\Documents\GitHub\nunit",
        MinChangeCount = 1
      };

      var sw = new Stopwatch();
      sw.Start();

      Run(new []
      {
        "-r", 
        @"C:\Users\grzes\Documents\GitHub\nscan",
        //@"C:\Users\grzes\Documents\GitHub\kafka\",
        //@"C:\Users\grzes\Documents\GitHub\botbuilder-dotnet\",
        "-b", "master",
        //"-b", "trunk",
        "--max-coupling-per-hospot", "20",
        "--max-hostpot-count", "100",
        "-o", "output.html",
        "--min-change-count", "1"
      });
      sw.Stop();
      System.Console.WriteLine("Total " + sw.Elapsed);
    }

    private static void Run(string[] args)
    {
      var analysisConfig = CommandLineParser.Parse(args);
      var analysisResult = GitRepoAnalysis.Analyze(
        analysisConfig.RepoPath,
        analysisConfig.Branch,
        analysisConfig.MinChangeCount,
        //DateTime.Now - TimeSpan.FromDays(366));
        SinceBeginning());
      
      var readyDocument = new HtmlAnalysisDocument(analysisConfig)
        .RenderString(analysisResult);
      
      File.WriteAllText(analysisConfig.OutputFile, readyDocument);
      Browser.Open(analysisConfig.OutputFile);
    }

    private static DateTime SinceBeginning()
    {
      return DateTime.MinValue + TimeSpan.FromDays(300);
    }
  }
}