using LibGit2Sharp;

namespace GitAnalysis
{
  public static class Extract
  {
    public static IBlob BlobFrom(TreeEntryChanges treeEntry, Commit currentCommit)
    {
      var blob = (Blob)currentCommit[treeEntry.Path].Target;
      if (blob.IsBinary)
      {
        return new BinaryBlob();
      }
      return new NonBinaryBlob(blob.GetContentText());
    }
  }
}