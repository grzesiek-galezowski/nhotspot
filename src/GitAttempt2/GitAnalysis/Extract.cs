using LibGit2Sharp;

namespace GitAnalysis
{
  public static class Extract
  {
    public static IBlob BlobFrom(TreeEntryChanges treeEntry, Commit currentCommit)
    {
      if (currentCommit[treeEntry.Path].TargetType == TreeEntryTargetType.GitLink)
      {
        return new UnsupportedBlob();
      }
      var blob = (Blob)currentCommit[treeEntry.Path].Target;
      if (blob.IsBinary)
      {
        return new UnsupportedBlob();
      }
      return new NonBinaryBlob(blob.GetContentText());
    }
  }
}