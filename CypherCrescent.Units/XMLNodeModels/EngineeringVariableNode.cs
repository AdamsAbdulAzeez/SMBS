using CypherCrescent.Units.Models;

namespace CypherCrescent.Units.XMLNodeModels
{
    public class EngineeringVariableNode
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DatabaseUnit_Id { get; set; }
        public int DefaultUnit_Id { get; set; }
        public int DatabaseUnitMultiplier_Id { get; set; }
        public int DefaultUnitMultiplier_Id { get; set; }
        public CategoryEnum Category { get; set; }
    }
}
