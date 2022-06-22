using System.Collections.Generic;
using AtmaFileSystem;
using Core.Maybe;

namespace NHotSpot.ApplicationLogic;

public class RepoAnalysis
{
    private readonly IClock _clock;
    private readonly int _minChangeCount;
    private readonly Maybe<RelativeDirectoryPath> _subfolder;

    public RepoAnalysis(IClock clock, int minChangeCount, Maybe<RelativeDirectoryPath> subfolder)
    {
        _clock = clock;
        _minChangeCount = minChangeCount;
        _subfolder = subfolder;
    }
    
    public AnalysisResult ExecuteOn(ISourceControlRepository sourceControlRepository)
    {
        var visitor = new CollectFileChangeRateFromCommitVisitor(_clock, _minChangeCount, _subfolder);
        sourceControlRepository.CollectResults(visitor);
        var analysisResult = CreateAnalysisResult(
            visitor.Result(), 
            sourceControlRepository.Path, 
            sourceControlRepository.TotalCommits);
        return analysisResult;
    }

    private static AnalysisResult CreateAnalysisResult(IEnumerable<IFileHistory> fileHistories, string repositoryPath,
        int totalCommits)
    {
        var packageHistoryNode = Rankings.GatherPackageTreeMetricsByPath(fileHistories);

        var flatPackageHistoriesByPath = Rankings.GatherFlatPackageHistoriesByPath(fileHistories);
        return new AnalysisResult(fileHistories, 
            flatPackageHistoriesByPath, 
            repositoryPath,
            packageHistoryNode, 
            ComplexityMetrics.CalculateCoupling
                <CouplingBetweenFiles, IFileHistory, RelativeFilePath>(
                    fileHistories, 
                    totalCommits),
            ComplexityMetrics.CalculateCoupling
                <CouplingBetweenPackages, IFlatPackageHistory, RelativeDirectoryPath>(
                    flatPackageHistoriesByPath.Values, 
                    totalCommits)
        );
    }
}