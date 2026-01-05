using System;
using AtmaFileSystem;
using TddXt.AnyRoot.Strings;
using static TddXt.AnyRoot.Root;

namespace ApplicationLogicSpecification.Automation;

public class DirProxy(RelativeDirectoryPath dirName, RepositoryEvolution context, CommitContext commitContext)
{
  public FileProxy File(string fileName)
  {
    return new FileProxy(dirName + FileName.Value(fileName), context, commitContext);
  }

  public DirProxy Dir(string subDirName)
  {
    return new DirProxy(dirName + RelativeDirectoryPath.Value(subDirName), context, commitContext);
  }

  public DirProxy Dir(string dirName, Action<DirProxy> dirProxyAction)
  {
    var dirProxy = Dir(dirName);
    dirProxyAction(dirProxy);
    return dirProxy;
  }

  public void Date(in DateTime date)
  {
    commitContext.Date = date;
  }
}

public class CommitContext
{
  public DateTime Date { get; set; } = DateTime.UtcNow;
  public string ChangeId { get; } = Any.String();
}
