using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationLogic
{
  public class FlatPackageChangeLog : IFlatPackageChangeLog
  {
    private readonly string _packagePath;
    private List<IFileChangeLog> _files = new List<IFileChangeLog>();

    public FlatPackageChangeLog(string packagePath)
    {
      _packagePath = packagePath;
    }

    public void Add(IFileChangeLog fileChangeLog)
    {
      _files.Add(fileChangeLog);
    }

    public IReadOnlyList<IFileChangeLog> Files => _files;

    public int ChangesCount()
    {
      return _files.Sum(f => f.ChangesCount());
    }

    public double ComplexityOfCurrentVersion()
    {
      return _files.Sum(f => f.ComplexityOfCurrentVersion());
    }

    public double HotSpotRating()
    {
      return _files.Sum(f => f.HotSpotRating());
    }

    public string PathOfCurrentVersion()
    {
      return _packagePath;
    }
  }
}