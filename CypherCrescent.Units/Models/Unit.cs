using System;
using System.Collections.Generic;
using System.Linq;
using CypherCrescent.Units.Contracts;

namespace CypherCrescent.Units.Models
{
    public class Unit : IEquatable<Unit>, IUnitInfo
    {
        protected Unit()
        {
            _multipliers = new HashSet<Multiplier>();
        }

        public static Unit CreateByName(string fullName)
        {
            return new Unit()
            {
                FullName = fullName
            };
        }
        //test here
        public static Unit New(string symbol, string fullName, UnitTypeEnum type, double multiplicand, double addend, Quantity quantity)
        {
            var unit = new Unit
            {
                Symbol = symbol,
                FullName = fullName,
                Type = type,
                Multiplicand = multiplicand,
                Addend = addend,
                Quantity = quantity
            };

            quantity.AddUnit(unit);

            return unit;
        }
        public virtual bool IsReadOnly { get; }

        public override string ToString()
        {
            return Symbol;
        }

        public virtual string GetSymbol(IMultiplierInfo multiplier)
        {
            var symbol = Symbol;

            if (multiplier != null)
            {
                //if we have a '\' or '/' stroke in the unit symbol we break it up into parts
                var firstPart = string.Empty;
                var secondPart = string.Empty;
                var delimeter = string.Empty;
                int index;
                if (Symbol.IndexOf(_backstroke) > 0 || Symbol.IndexOf(_frontstroke) > 0)
                {
                    index = Symbol.IndexOf(_backstroke);
                    if (index > 0)
                    {
                        firstPart = Symbol.Substring(0, index);
                        secondPart = Symbol.Substring(index + 1, Symbol.Length - index - 1);
                        delimeter = _backstroke.ToString();
                    }
                    else if (Symbol.IndexOf(_frontstroke) > 0)
                    {
                        index = Symbol.IndexOf(_frontstroke);
                        firstPart = Symbol.Substring(0, index);
                        secondPart = Symbol.Substring(index + 1, Symbol.Length - index - 1);
                        delimeter = _frontstroke.ToString();
                    }


                }
                else
                {
                    firstPart = Symbol;
                }

                var topMultiplier = string.Empty;
                var bottomMultiplier = string.Empty;
                if (multiplier.Symbol.IndexOf(_backstroke) > 0 || multiplier.Symbol.IndexOf(_frontstroke) > 0)
                {
                    index = multiplier.Symbol.IndexOf(_backstroke);
                    if (index > 0)
                    {
                        topMultiplier = multiplier.Symbol.Substring(0, index);
                        bottomMultiplier = multiplier.Symbol.Substring(index + 1, multiplier.Symbol.Length - index - 1);

                    }
                    else if (multiplier.Symbol.IndexOf(_frontstroke) > 0)
                    {
                        index = multiplier.Symbol.IndexOf(_frontstroke);
                        topMultiplier = multiplier.Symbol.Substring(0, index);
                        bottomMultiplier = multiplier.Symbol.Substring(index + 1, multiplier.Symbol.Length - index - 1);
                    }

                    var reset = false;
                    foreach (var ch in topMultiplier)
                    {
                        if (char.IsDigit(ch))
                        {
                            reset = true;
                            break;
                        }
                    }
                    if (reset)
                    {
                        topMultiplier = string.Empty;
                    }

                    reset = false;
                    foreach (var ch in bottomMultiplier)
                    {
                        if (char.IsDigit(ch))
                        {
                            reset = true;
                            break;
                        }
                    }
                    if (reset)
                    {
                        bottomMultiplier = string.Empty;
                    }
                }
                else
                {
                    topMultiplier = multiplier.Symbol;
                }

                firstPart = AppendUnitAndMultiplier(firstPart, topMultiplier);
                secondPart = AppendUnitAndMultiplier(secondPart, bottomMultiplier);
                symbol = firstPart + delimeter + secondPart;
            }

            return symbol;
        }

        public virtual bool Equals(Unit other)
        {
            if (other == null) return false;
            return Symbol == other.Symbol;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Unit)) return false;
            return Equals(obj as Unit);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public virtual Multiplier GetMultiplier(string multiplierSymbol)
        {
            var multiplier = Multipliers.ToList().Find(m => m.Symbol == multiplierSymbol);
            return multiplier;
        }

        public virtual void AddMultiplier(Multiplier multiplier)
        {
            if (multiplier != null) _multipliers.Add(multiplier);

        }

        public virtual string AppendUnitAndMultiplier(string unitSymbol, string multSymbol)
        {
            var symbol = string.Empty;
            var addParenthesis = false;
            if (unitSymbol.Contains(_left_parenthesis) && unitSymbol.Contains(_right_parenthesis))
            {
                addParenthesis = true;
            }
            else
            {
                if (multSymbol.Contains(_left_parenthesis) && multSymbol.Contains(_right_parenthesis))
                {
                    addParenthesis = true;
                }
            }
            unitSymbol.Trim();
            unitSymbol.Trim(new char[] { _left_parenthesis, _right_parenthesis });
            multSymbol.Trim(new char[] { _left_parenthesis, _right_parenthesis });
            multSymbol.Trim();

            if (addParenthesis)
            {
                symbol += _left_parenthesis + multSymbol + unitSymbol + _right_parenthesis;
            }
            else
            {
                symbol += multSymbol + unitSymbol;
            }
            return symbol;
        }
        public virtual string Symbol { get; set; }
        public virtual string FullName { get; set; }
        public virtual UnitTypeEnum Type { get; set; }
        public virtual double Multiplicand { get; set; }
        public virtual double Addend { get; set; }
        public virtual Quantity Quantity { get; set; }

        private readonly ISet<Multiplier> _multipliers;
        public virtual IEnumerable<Multiplier> Multipliers => _multipliers;
        public virtual int Id { get; set; }
        private static readonly char _backstroke = '\\';
        private static readonly char _frontstroke = '/';
        private static readonly char _left_parenthesis = '(';
        private static readonly char _right_parenthesis = ')';

    }
}
