using CypherCrescent.Units.Contracts;

namespace CypherCrescent.Units.Services
{
    public class UnitDisplayValue
    {
        public UnitDisplayValue(double? value, UnitSelectionOption unitInfo)
        {
            Value = value;
            UnitInfo = unitInfo;
        }

        public double? Value { get; }
        public UnitSelectionOption UnitInfo { get; }
    }
}
