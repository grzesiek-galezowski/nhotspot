using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ApplicationLogic
{
  public class FlatPackageChangeLog : IFlatPackageChangeLog
  {
    private readonly string _packagePath;
    private readonly List<IFileChangeLog> _files = new List<IFileChangeLog>();

    public FlatPackageChangeLog(string packagePath)
    {
      _packagePath = packagePath;
    }

    public void Add(IFileChangeLog fileChangeLog)
    {
      _files.Add(fileChangeLog);
    }

    public IEnumerable<IFileChangeLog> Files => _files;

    public int ChangesCount()
    {
      var changes = _files.Select(f => f.ChangeIds()).SelectMany(s => s).Distinct();
      return changes.Count();
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

    public string Name()
    {
      return Path.GetFileName(_packagePath);
    }

    public override string ToString()
    {
      return $"{nameof(_packagePath)}: {_packagePath}";
    }
  }
}