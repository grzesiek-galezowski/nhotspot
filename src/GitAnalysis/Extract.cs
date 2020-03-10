using LibGit2Sharp;

namespace NHotSpot.GitAnalysis
{
  public static class Extract
  {
    public static IBlob BlobFrom(Commit currentCommit, string treeEntryPath)
    {
      if (currentCommit[treeEntryPath].TargetType == TreeEntryTargetType.GitLink)
      {
        return new UnsupportedBlob();
      }
      var blob = (Blob)currentCommit[treeEntryPath].Target;
      if (blob.IsBinary)
      {
        return new SupportedBlob(string.Empty);
      }
      return new SupportedBlob(blob.GetContentText());
    }
  }
}