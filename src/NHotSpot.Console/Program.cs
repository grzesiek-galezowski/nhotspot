using System.IO;
using NHotSpot.GitAnalysis;
using NHotSpot.ResultRendering;
using Core.NullableReferenceTypesExtensions;

namespace NHotSpot.Console;

public static class Program
{
  public static void Run(string[] args)
  {
    var analysisConfigOrError = CommandLineParser.Parse(args);

    analysisConfigOrError.Match(
        analysisConfig =>
        {
          using var gitRepoAnalysis = new GitRepoAnalysis(analysisConfig.RepoPath.OrThrow());
          var analysisResult = gitRepoAnalysis.Analyze(
                analysisConfig.Subfolder,
                analysisConfig.Branch.OrThrow(),
                analysisConfig.MinChangeCount,
                analysisConfig.StartDate);

          var readyDocument = new HtmlAnalysisDocument(analysisConfig)
                  .RenderString(analysisResult, string.Join(" ", args));

          var reportPath = analysisConfig.OutputFile.OrThrow();
          File.WriteAllText(reportPath, readyDocument);
          Browser.Open(reportPath);
        },
        result =>
        {
          System.Console.Error.WriteLine(result.ErrorText);
        });
  }
}
