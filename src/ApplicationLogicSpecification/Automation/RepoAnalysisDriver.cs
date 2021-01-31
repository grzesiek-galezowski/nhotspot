using System;
using AtmaFileSystem;
using Functional.Maybe;
using NHotSpot.ApplicationLogic;
using static TddXt.AnyRoot.Root;

namespace ApplicationLogicSpecification.Automation
{
  public class RepoAnalysisDriver
  {
    public AnalysisResult Analyze(Action<IRepositoryEvolution> action1)
    {
      var analysisResult =
        new RepoAnalysis(
            Any.Instance<IClock>(),
            0,
            Maybe<RelativeDirectoryPath>.Nothing)
          .ExecuteOn(
            new MockSourceControlRepository("REPO", action1));
      return analysisResult;
    }
  }
}