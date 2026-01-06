using System;
using AtmaFileSystem;
using NHotSpot.ApplicationLogic;

namespace ApplicationLogicSpecification.Automation;

public class RepositoryEvolution(ITreeVisitor visitor) : IRepositoryEvolution
{
  private int _commits = 0;

  public void Modify(Change change)
  {
    visitor.OnModified(change);
  }

  public void Rename(RelativeFilePath oldPath, Change change)
  {
    visitor.OnRenamed(oldPath, change);
  }

  public void Copy(Change change)
  {
    visitor.OnCopied(change);
  }

  public void Add(Change change)
  {
    visitor.OnAdded(change);
  }

  public void Remove(RelativeFilePath removedEntryPath)
  {
    visitor.OnRemoved(removedEntryPath);
  }

  public void CommitChanges()
  {
    _commits++;
  }

  public int CommitCount()
  {
    return _commits;
  }

  public void Commit(Action<DirProxy> action)
  {
    var relativeDirectoryPath = RelativeDirectoryPath.Value("");
    var dirProxy = new DirProxy(relativeDirectoryPath, this, new CommitContext());
    action(dirProxy);
    CommitChanges();
  }
}
