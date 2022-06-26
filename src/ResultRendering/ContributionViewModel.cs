namespace NHotSpot.ResultRendering;

public record ContributionViewModel(
  decimal ChangePercentage, 
  int ChangeCount, 
  string AuthorName);