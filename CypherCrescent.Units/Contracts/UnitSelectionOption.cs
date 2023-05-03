namespace CypherCrescent.Units.Contracts
{
    public struct UnitSelectionOption
    {
        public UnitSelectionOption(IUnitInfo unit, IMultiplierInfo multiplier)
        {
            Unit = unit;
            Multiplier = multiplier;
            DisplayText = $"({Unit?.GetSymbol(Multiplier)})";
        }

        public override string ToString() => DisplayText;

        public string DisplayText { get; internal set; }
        public IUnitInfo Unit { get; }
        public IMultiplierInfo Multiplier { get; }
    }
}
