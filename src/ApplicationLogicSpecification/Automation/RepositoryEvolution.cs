using System;
using AtmaFileSystem;
using NHotSpot.ApplicationLogic;

namespace ApplicationLogicSpecification.Automation;

public class RepositoryEvolution : IRepositoryEvolution
{
    private readonly ITreeVisitor _visitor;
    private int _commits = 0;

    public RepositoryEvolution(ITreeVisitor visitor)
    {
        _visitor = visitor;
    }

    public void Modify(Change change)
    {
        _visitor.OnModified(change);
    }

    public void Rename(RelativeFilePath oldPath, Change change)
    {
        _visitor.OnRenamed(oldPath, change);
    }

    public void Copy(Change change)
    {
        _visitor.OnCopied(change);
    }

    public void Add(Change change)
    {
        _visitor.OnAdded(change);
    }

    public void Remove(RelativeFilePath removedEntryPath)
    {
        _visitor.OnRemoved(removedEntryPath);
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
        this.CommitChanges();
    }
}
