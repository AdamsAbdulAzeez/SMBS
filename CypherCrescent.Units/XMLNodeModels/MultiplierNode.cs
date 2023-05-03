using CypherCrescent.Units.Models;

namespace CypherCrescent.Units.XMLNodeModels
{
    public class MultiplierNode
    {
        public int Id { get; set; }
        public int UnitId { get; set; }
        public string MultiplierSymbol { get; set; }
        public double Multiplicand { get; set; }
        public virtual MultiplierTypeEnum MultiplierType { get; set; }
    }
}
