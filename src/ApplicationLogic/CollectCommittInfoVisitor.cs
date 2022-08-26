using System;
using System.Collections.Generic;
using System.Linq;

namespace NHotSpot.ApplicationLogic;

public class CollectCommittInfoVisitor : ICollectCommittInfoVisitor
{
  private readonly Dictionary<string, int> _committCountPerAuthor = new();
  private int _totalContributions = 0;

  public void AddMetadata(string authorName, DateTimeOffset date)
  {
    if (!_committCountPerAuthor.ContainsKey(authorName))
    {
      _committCountPerAuthor[authorName] = 0;
    }

    _committCountPerAuthor[authorName]++;
    _totalContributions++;
  }

  public List<Contribution> TotalContributions()
  {
    return _committCountPerAuthor
      .Select(kvp => new Contribution(kvp.Key, kvp.Value, _totalContributions))
      .ToList();
  }
}
