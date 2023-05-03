namespace CypherCrescent.Units.Contracts
{
    public interface IUnitInfo
    {
        int Id { get; set; }
        string GetSymbol(IMultiplierInfo multiplier);
    }
}
