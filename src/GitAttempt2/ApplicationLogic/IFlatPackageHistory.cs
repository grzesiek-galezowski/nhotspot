using System.Collections.Generic;
using AtmaFileSystem;

namespace ApplicationLogic
{
  public interface IFlatPackageHistory : IItemHistory<RelativeDirectoryPath>
  {
    void Add(IFileHistory fileHistory);
    IEnumerable<IFileHistory> Files { get; }
    DirectoryName Name();
  }
}