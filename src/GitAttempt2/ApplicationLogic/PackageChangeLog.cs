using System;

namespace ApplicationLogic
{
  public class PackageChangeLog : IPackageChangeLog
  {
    private readonly string _packagePath;
    private int _changeCount;
    private double _complexity;
    private int _changeCountRank = -1; //bug handle change in another way
    private int _complexityRank; //bug handle change in another way

    public PackageChangeLog(string packagePath)
    {
      _packagePath = packagePath;
    }

    public void AddMetricsFrom(IFileChangeLog fileChangeLog)
    {
      _complexity = ComplexityOfCurrentVersion() + fileChangeLog.ComplexityOfCurrentVersion();
      _changeCount = ChangesCount() + fileChangeLog.ChangesCount();
    }

    public int ChangesCount()
    {
      return _changeCount;
    }

    public double ComplexityOfCurrentVersion()
    {
      return _complexity;
    }

    public double HotSpotRank()
    {
      return ComplexityMetrics.CalculateHotSpotRank(_complexityRank, _changeCountRank);
    }

    public string PathOfCurrentVersion()
    {
      return _packagePath;
    }

    public void AssignChangeCountRank(int rank)
    {
      _changeCountRank = rank;
    }

    public void AssignComplexityRank(int complexityRank)
    {
      _complexityRank = complexityRank;
    }

  }
}