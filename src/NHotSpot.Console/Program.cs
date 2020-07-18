using System;
using System.IO;
using NHotSpot.GitAnalysis;
using NHotSpot.ResultRendering;
using NullableReferenceTypesExtensions;

namespace NHotSpot.Console
{
    internal static class Program
    {
        public static void Run(string[] args)
        {
            var analysisConfig = CommandLineParser.Parse(args);
            using var gitRepoAnalysis = new GitRepoAnalysis(analysisConfig.RepoPath.OrThrow());
            var analysisResult = gitRepoAnalysis.Analyze(analysisConfig.Subfolder,
                analysisConfig.Branch.OrThrow(),
                analysisConfig.MinChangeCount,
                analysisConfig.StartDate);
      
            var readyDocument = new HtmlAnalysisDocument(analysisConfig)
                .RenderString(analysisResult);
      
            File.WriteAllText(analysisConfig.OutputFile, readyDocument);
            Browser.Open(analysisConfig.OutputFile.OrThrow());
        }

        private static DateTime SinceBeginning()
        {
            return DateTime.MinValue + TimeSpan.FromDays(300);
        }
    }
}