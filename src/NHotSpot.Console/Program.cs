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
            var analysisResult = GitRepoAnalysis.Analyze(
                analysisConfig.RepoPath.OrThrow(),
                analysisConfig.Branch.OrThrow(),
                analysisConfig.MinChangeCount,
                //DateTime.Now - TimeSpan.FromDays(366));
                SinceBeginning());
      
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