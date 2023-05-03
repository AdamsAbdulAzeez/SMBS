using System.Collections.Generic;
using System.Linq;

namespace CypherCrescent.Units.Models
{
    public class Quantity
    {
        protected Quantity()
        {
            _units = new HashSet<Unit>();
        }

        public static Quantity New(string name)
        {
            return new Quantity
            {
                Name = name
            };
        }

        public virtual Unit GetUnit(int unitId)
        {
            return _units.FirstOrDefault(unit => unit.Id == unitId);
        }

        public virtual void AddUnit(Unit unit)
        {
            if (unit != null) _units.Add(unit);
        }
        public override string ToString()
        {
            return Name;
        }

        public virtual string Name { get; set; }
        public virtual bool IsReadOnly { get; set; }
        public virtual Unit BaseUnit { get; set; }
        private readonly ISet<Unit> _units;
        public virtual IEnumerable<Unit> Units => _units;
        public virtual int Id { get; set; }
    }
}
