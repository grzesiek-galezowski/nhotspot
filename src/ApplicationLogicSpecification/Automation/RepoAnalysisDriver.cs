using System;
using ApplicationLogic;
using TddXt.AnyRoot;

namespace ApplicationLogicSpecification.Automation
{
  public class RepoAnalysisDriver
  {
    public AnalysisResult Analyze(Action<IRepositoryEvolution> action1)
    {
      var analysisResult = new RepoAnalysis(Root.Any.Instance<IClock>(), 0).ExecuteOn(
        new MockSourceControlRepository("REPO", action1));
      return analysisResult;
    }
  }
}