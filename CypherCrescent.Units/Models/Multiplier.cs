using System;
using CypherCrescent.Units.Contracts;

namespace CypherCrescent.Units.Models
{
    public class Multiplier : IEquatable<Multiplier>, IMultiplierInfo
    {
        public virtual bool Equals(Multiplier other)
        {
            if (other == null) return false;
            return Id == other.Id;
        }
        public static Multiplier New(string symbol, double multiplicand, MultiplierTypeEnum multiplierType, Unit unit)
        {
            var multiplier = new Multiplier
            {
                Symbol = symbol,
                Multiplicand = multiplicand,
                MultiplierType = multiplierType,
                Unit = unit,
            };

            unit.AddMultiplier(multiplier);

            return multiplier;
        }
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Multiplier)) return false;
            return Equals(obj as Multiplier);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return Symbol;
        }
        public virtual string Symbol { get; set; }
        public virtual double Multiplicand { get; set; }
        public virtual MultiplierTypeEnum MultiplierType { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual bool IsReadOnly { get; set; }
        public virtual int Id { get; set; }
    }
}
