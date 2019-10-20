namespace ApplicationLogic
{
  public class AnalysisConfig
  {
    public int MaxHotSpotCount { get; set; }
    public int MaxCouplingsPerHotSpot { get; set; }
    public string OutputFile { get; set; }
    public string RepoPath { get; set; }
    public string Branch { get; set; }
  }
}