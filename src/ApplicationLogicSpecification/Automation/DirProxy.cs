using System;
using AtmaFileSystem;
using TddXt.AnyRoot.Strings;
using static TddXt.AnyRoot.Root;

namespace ApplicationLogicSpecification.Automation
{
  public class DirProxy
  {
    private readonly RelativeDirectoryPath _dirName;
    private readonly RepositoryEvolution _context;
    private CommitContext _commitContext;

    public DirProxy(RelativeDirectoryPath dirName, RepositoryEvolution context, CommitContext commitContext)
    {
      _dirName = dirName;
      _context = context;
      _commitContext = commitContext;
    }

    public FileProxy File(string fileName)
    {
      return new FileProxy(_dirName + FileName.Value(fileName), _context, _commitContext);
    }

    public DirProxy Dir(string subDirName)
    {
      return new DirProxy(_dirName + RelativeDirectoryPath.Value(subDirName), _context, _commitContext);
    }

    public DirProxy Dir(string dirName, Action<DirProxy> dirProxyAction)
    {
      var dirProxy = Dir(dirName);
      dirProxyAction(dirProxy);
      return dirProxy;
    }

    public void Date(in DateTime date)
    {
      _commitContext.Date = date;
    }
  }

  public class CommitContext
  {
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public string ChangeId { get; } = Any.String();
  }
}