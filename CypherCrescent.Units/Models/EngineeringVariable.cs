using System;
using CypherCrescent.Units.Contracts;

namespace CypherCrescent.Units.Models
{
    public class EngineeringVariable
    {
        protected EngineeringVariable() { }
        public static EngineeringVariable New(string name, Unit databaseUnit, Unit defaultUnit, Multiplier databaseMultiplier, Multiplier defaultMultiplier)
        {
            return new EngineeringVariable
            {
                Name = name,
                DatabaseUnit = databaseUnit,
                DefaultUnit = defaultUnit,
                DatabaseMultiplier = databaseMultiplier,
                DefaultMultiplier = defaultMultiplier,
            };
        }

        
        public event Action<UnitSelectionOption> DisplayUnitChanged;
        public virtual string Name { get; set; }
        public virtual Unit DatabaseUnit { get; set; }
        public virtual Unit DefaultUnit { get; set; }
        public virtual Multiplier DatabaseMultiplier { get; set; }
        public virtual Multiplier DefaultMultiplier { get; set; }
        public virtual CategoryEnum Category { get; set; }
        public virtual int Id { get; set; }
    }
    public enum CategoryEnum { Annulus, Component, Reservoir, SurfaceFacility, Well }
}
