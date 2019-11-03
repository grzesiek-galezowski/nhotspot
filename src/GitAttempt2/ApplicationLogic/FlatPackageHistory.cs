using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AtmaFileSystem;

namespace ApplicationLogic
{
  public class FlatPackageHistory : IFlatPackageHistory
  {
    private readonly RelativeDirectoryPath _packagePath;
    private readonly List<IFileHistory> _files = new List<IFileHistory>();

    public FlatPackageHistory(RelativeDirectoryPath packagePath)
    {
      _packagePath = packagePath;
    }

    public void Add(IFileHistory fileHistory)
    {
      _files.Add(fileHistory);
    }

    public IEnumerable<IFileHistory> Files => _files;

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

    public RelativeDirectoryPath PathOfCurrentVersion()
    {
      return _packagePath;
    }

    public DirectoryName Name()
    {
      return _packagePath.DirectoryName();
    }

    public override string ToString()
    {
      return $"{nameof(_packagePath)}: {_packagePath}";
    }
  }
}