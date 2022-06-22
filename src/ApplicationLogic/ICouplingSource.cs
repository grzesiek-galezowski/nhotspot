namespace NHotSpot.ApplicationLogic;

public interface ICouplingSource<TCoupling, THistory>  where THistory : ICouplingSource<TCoupling, THistory>
{
    TCoupling CalculateCouplingTo(THistory otherHistory, int totalCommits);
}