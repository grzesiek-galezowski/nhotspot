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
    private readonly HashSet<string> _changeIds = new HashSet<string>();

    public FlatPackageHistory(RelativeDirectoryPath packagePath)
    {
      _packagePath = packagePath;
    }

    public void Add(IFileHistory fileHistory)
    {
      _files.Add(fileHistory);
      _changeIds.UnionWith(fileHistory.ChangeIds());
    }

    public IEnumerable<IFileHistory> Files => _files;

    public int ChangesCount()
    {
      return _changeIds.Count;
    }

    public double ComplexityOfCurrentVersion()
    {
      return _files.Sum(f => f.ComplexityOfCurrentVersion());
    }

    public double HotSpotRating()
    {
      //bug 

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