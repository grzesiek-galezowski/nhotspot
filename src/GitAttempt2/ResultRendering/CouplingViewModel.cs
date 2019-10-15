namespace ResultRendering
{
  public class CouplingViewModel
  {
    public string Left { get; }
    public string Right { get; }
    public int CouplingCount { get; }

    public CouplingViewModel(string left, string right, int couplingCount)
    {
      Left = left;
      Right = right;
      CouplingCount = couplingCount;
    }
  }
}