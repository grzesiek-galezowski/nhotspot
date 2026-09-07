namespace NHotSpot.ApplicationLogic;

public interface ICouplingSource<TCoupling, THistory> where THistory : ICouplingSource<TCoupling, THistory>
{
  int CalculateCouplingCountTo(THistory otherHistory);
  TCoupling CalculateCouplingTo(THistory otherHistory, int totalCommits, int couplingCount);
}
