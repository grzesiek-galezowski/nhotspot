using System;
using AtmaFileSystem;
using Core.Maybe;
using NHotSpot.ApplicationLogic;
using static TddXt.AnyRoot.Root;

namespace ApplicationLogicSpecification.Automation;

public class RepoAnalysisDriver
{
    public IClock Clock { get; set; }

    public RepoAnalysisDriver()
    {
        Clock = Any.Instance<IClock>();
    }

    public AnalysisResult Analyze(Action<IRepositoryEvolution> action1)
    {
        var analysisResult =
            new RepoAnalysis(
                    Clock,
                    0,
                    Maybe<RelativeDirectoryPath>.Nothing)
                .ExecuteOn(
                    new MockSourceControlRepository("REPO", action1));
        return analysisResult;
    }
}