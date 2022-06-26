namespace NHotSpot.ApplicationLogic;

public record Contribution
{
  public Contribution(string authorName, int commitsByAuthor, int totalFileCommits)
  {
    ChangeCount = commitsByAuthor;
    ChangePercentage = commitsByAuthor / (decimal) totalFileCommits*100;
    AuthorName = authorName;
  }

  public string AuthorName { get; }
  public decimal ChangePercentage { get; }
  public int ChangeCount { get; }
}