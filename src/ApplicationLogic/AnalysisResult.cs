﻿using System;
using System.Collections.Generic;
using System.Linq;
using AtmaFileSystem;

namespace NHotSpot.ApplicationLogic;

public class AnalysisResult
{
    private readonly IPackageHistoryNode _packageHistoryRootNode;
    private readonly IEnumerable<CouplingBetweenFiles> _fileCouplings;
    private readonly IEnumerable<CouplingBetweenPackages> _packageCouplings; //bug use it!
    private readonly IOrderedEnumerable<IFileHistory> _entriesByHotSpotRating;
    private readonly IEnumerable<IFileHistory> _entriesByDiminishingComplexity;
    private readonly IOrderedEnumerable<IFileHistory> _entriesByDiminishingChangesCount;
    private readonly IOrderedEnumerable<IFileHistory> _entriesByDiminishingActivityPeriod;
    private readonly IOrderedEnumerable<IFileHistory> _entriesFromMostRecentlyChanged;
    private readonly IEnumerable<IFileHistory> _entriesFromMostAncientlyChanged;
    private readonly IOrderedEnumerable<IFlatPackageHistory> _packagesByDiminishingHotSpotRating;

    public AnalysisResult(IEnumerable<IFileHistory> fileHistories,
      List<Contribution> totalContributions,
      Dictionary<RelativeDirectoryPath, IFlatPackageHistory> packageHistoriesByPath,
      string normalizedPathToRepository,
      IPackageHistoryNode packageHistoryRootNode,
      IEnumerable<CouplingBetweenFiles> fileCouplings,
      IEnumerable<CouplingBetweenPackages> packageCouplings)
    {
      TotalContributions = totalContributions;
      PathToRepository = normalizedPathToRepository;
        _packageHistoryRootNode = packageHistoryRootNode;
        _fileCouplings = fileCouplings;
        _packageCouplings = packageCouplings;
        _entriesByHotSpotRating = fileHistories.OrderByDescending(h => h.HotSpotRating());
        _entriesByDiminishingComplexity = fileHistories.OrderByDescending(h => h.ComplexityOfCurrentVersion());
        _entriesByDiminishingChangesCount = fileHistories.OrderByDescending(h => h.ChangesCount());
        _entriesByDiminishingActivityPeriod = fileHistories.OrderByDescending(h => h.ActivityPeriod());
        _entriesFromMostRecentlyChanged = fileHistories.OrderByDescending(h => h.LastChangeDate());
        _entriesFromMostAncientlyChanged = _entriesFromMostRecentlyChanged.Reverse();
        _packagesByDiminishingHotSpotRating = packageHistoriesByPath.Select(kv => kv.Value).OrderByDescending(cl => cl.HotSpotRating());
    }

    public List<Contribution> TotalContributions { get; }
    public string PathToRepository { get; }

    public IPackageHistoryNode PackageTree()
    {
        return _packageHistoryRootNode;
    }

    public IEnumerable<CouplingBetweenFiles> FileCouplingMetrics()
    {
        return _fileCouplings;
    }

    public IEnumerable<CouplingBetweenPackages> PackageCouplingMetrics()
    {
        return _packageCouplings;
    }

    public IEnumerable<IFileHistory> EntriesByHotSpotRating()
    {
        return _entriesByHotSpotRating;
    }

    public IEnumerable<IFileHistory> EntriesByDiminishingComplexity()
    {
        return _entriesByDiminishingComplexity;
    }

    public IEnumerable<IFileHistory> EntriesByDiminishingChangesCount()
    {
        return _entriesByDiminishingChangesCount;
    }

    public IEnumerable<IFileHistory> EntriesByDiminishingActivityPeriod()
    {
        return _entriesByDiminishingActivityPeriod;
    }

    public IEnumerable<IFileHistory> EntriesFromMostRecentlyChanged()
    {
        return _entriesFromMostRecentlyChanged;
    }

    public IEnumerable<IFileHistory> EntriesFromMostAncientlyChanged()
    {
        return _entriesFromMostAncientlyChanged;
    }

    public IEnumerable<IFlatPackageHistory> PackagesByDiminishingHotSpotRating()
    {
        return _packagesByDiminishingHotSpotRating;
    }

    public IEnumerable<Contribution> ContributorsByOwnershipPerSingleFileChange()
    {
      var contributionsByAuthor = new List<Contribution>();
      var allContributions = _entriesFromMostRecentlyChanged
        .SelectMany(e => e.Contributions()).ToList();
      var dictionary = allContributions
        .GroupBy(c => c.AuthorName).ToDictionary(g => g.Key, g => g.Select(e => e));
      var allContributionsCount = allContributions.Sum(c => c.ChangeCount);
      foreach (var (author, contributions) in dictionary)
      {
        var authorContributions = contributions.Sum(c => c.ChangeCount);
        contributionsByAuthor.Add(new Contribution(author, authorContributions, allContributionsCount));
      }

      return contributionsByAuthor;
    }
}
