using System.Collections.Generic;
using AtmaFileSystem;

namespace NHotSpot.ApplicationLogic;

public interface IFlatPackageHistory : 
    IItemHistory<RelativeDirectoryPath>, 
    ICouplingSource<CouplingBetweenPackages, IFlatPackageHistory>
{
    void Add(IFileHistory fileHistory);
    IEnumerable<IFileHistory> Files { get; }
    IEnumerable<string> ChangeIds();
}