namespace ApplicationLogic
{
  public class Coupling
  {
    public string Left { get; }
    public string Right { get; }
    public int CouplingCount { get; }

    public Coupling(string left, string right, int couplingCount)
    {
      Left = left;
      Right = right;
      CouplingCount = couplingCount;
    }
  }
}