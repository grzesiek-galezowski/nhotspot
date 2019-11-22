using System;
using AtmaFileSystem;

namespace ApplicationLogicSpecification
{
  public class DirProxy
  {
    private readonly RelativeDirectoryPath _dirName;
    private readonly RepositoryEvolution _context;

    public DirProxy(RelativeDirectoryPath dirName, RepositoryEvolution context)
    {
      _dirName = dirName;
      _context = context;
    }

    public FileProxy File(string fileName)
    {
      return new FileProxy(_dirName + FileName.Value(fileName), _context);
    }

    public DirProxy Dir(string subDirName)
    {
      return new DirProxy(_dirName + RelativeDirectoryPath.Value(subDirName), _context);
    }

    public DirProxy Dir(string dirName, Action<DirProxy> dirProxyAction)
    {
      var dirProxy = Dir(dirName);
      dirProxyAction(dirProxy);
      return dirProxy;
    }
  }
}