using System.Collections.Generic;
using AtmaFileSystem;

namespace NHotSpot.ApplicationLogic
{
  public interface IFlatPackageHistory : IItemHistory<RelativeDirectoryPath>
  {
    void Add(IFileHistory fileHistory);
    IEnumerable<IFileHistory> Files { get; }
  }
}