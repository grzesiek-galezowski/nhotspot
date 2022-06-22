namespace NHotSpot.ResultRendering;

public class ContributionViewModel
{
    public decimal ChangePercentage { get; }
    public int ChangeCount { get; }
    public string AuthorName { get; }

    public ContributionViewModel(decimal changePercentage, int changeCount, string authorName)
    {
        ChangePercentage = changePercentage;
        ChangeCount = changeCount;
        AuthorName = authorName;
    }
}