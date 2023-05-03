using CypherCrescent.Units.Models;

namespace CypherCrescent.Units.XMLNodeModels
{
    public class UnitNode
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public string FullName { get; set; }
        public UnitTypeEnum Type { get; set; }
        public int Quantity_Id { get; set; }
        public double Multiplicand { get; set; }
        public double Addend { get; set; }
    }
}
