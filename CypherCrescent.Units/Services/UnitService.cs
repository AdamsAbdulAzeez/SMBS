using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CypherCrescent.Units.Contracts;
using CypherCrescent.Units.Configuration;
using CypherCrescent.Units.Models;
using CypherCrescent.Units.Variables;

namespace CypherCrescent.Units.Services
{
    public sealed class UnitService : IUnitService
    {
        public Task InitialiseInFlatFileModeAsync(string appDirectory)
        {
            InMemoryStore = new UnitSystemStore();
            return InMemoryStore.InitialiseInFlatFileModeAsync(appDirectory);
        }

        public void RegisterVariable(VariableBase variable)
        {
            if (variable is Dimensionless)
            {
                variable.DisplayValue = variable.DatabaseValue;
                return;
            }

            var variableModel = InMemoryStore.EngineeringVariableCollection[variable.Name];

            variable.DatabaseUnit =
                new UnitSelectionOption(variableModel.DatabaseUnit, variableModel.DatabaseMultiplier);

            variable.DisplayUnit =
                new UnitSelectionOption(variableModel.DefaultUnit, variableModel.DefaultMultiplier);

            variable.DisplayValue = Convert(variable.DatabaseUnit, variable.DisplayUnit, variable.DatabaseValue);
        }

        public Task InitialiseFromSQLServer()
        {
            throw new NotImplementedException();
        }

        public Task InitialiseFromSQLite()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UnitSelectionOption> GetUnitOptions(string variableName)
        {
            var variable = InMemoryStore.EngineeringVariableCollection[variableName];
            var notAVariable = variable == null;

            if (notAVariable) return new List<UnitSelectionOption>();

            var unitOptions = new List<UnitSelectionOption>();

            if (variable.DatabaseUnit != null)
            {
                var dbUnit = variable.DatabaseUnit;
                var quantity = dbUnit.Quantity;
                unitOptions = GetQuantityUnits(quantity);
            }

            return unitOptions;
        }

        public double? Convert(UnitSelectionOption from, UnitSelectionOption to, double? value)
        {
            if (value == null) return value;
            //TODO: Test
            var fromUnit = InMemoryStore.UnitsNodeCollection[from.Unit.Id];
            var toUnit = InMemoryStore.UnitsNodeCollection[to.Unit.Id];

            var fromQuantity = fromUnit.Quantity;
            var toQuantity = toUnit.Quantity;


            var fromMultiplier = from.Multiplier != null ? InMemoryStore.MultiplierCollection[from.Multiplier.Id] : null;
            var toMultiplier = to.Multiplier != null ? InMemoryStore.MultiplierCollection[to.Multiplier.Id] : null;

            if (fromQuantity.Id != toQuantity.Id)
                throw new Exception("Cannot convert between units of different quantities.");

            if (fromMultiplier != null && fromMultiplier.Unit == fromUnit)
            {
                value *= fromMultiplier.Multiplicand;
            }
            var valueInBaseUnit = (value.Value - fromUnit.Addend) / fromUnit.Multiplicand;
            var valueInToUnit = (valueInBaseUnit * toUnit.Multiplicand) + toUnit.Addend;

            if (toMultiplier != null && toMultiplier.Unit == toUnit && toMultiplier.Multiplicand != 0)
            {
                valueInToUnit /= toMultiplier.Multiplicand;
            }
            return valueInToUnit;
        }
        //TODO: Test
        public double? ToDatabaseUnit(UnitSelectionOption incomingUnit, double? value, string variableName)
        {
            if (value == null) return value;

            var variable = InMemoryStore.GetVariable(variableName);
            if (variable == null) return value;

            var dbUnit = variable.DatabaseUnit;
            if (dbUnit == null) return value;

            var toUnit = new UnitSelectionOption(dbUnit, variable.DatabaseMultiplier);

            return Convert(incomingUnit, toUnit, value);
        }

        //TODO: Test
        public double? ToApplicationUnit(double dbValue, string variableName)
        {
            var variable = InMemoryStore.GetVariable(variableName);
            if (variable == null) return dbValue;

            var appUnit = variable.DefaultUnit;
            if (appUnit == null) return dbValue;

            var applicationUnit = new UnitSelectionOption(appUnit, variable.DefaultMultiplier);
            var dbUnit = new UnitSelectionOption(variable.DatabaseUnit, variable.DatabaseMultiplier);

            return Convert(dbUnit, applicationUnit, dbValue);
        }

        private UnitSelectionOption GetSourceUnitOption(Unit sourceUnit, string multiplierSymbol)
        {
            if (multiplierSymbol == null)
            {
                return new UnitSelectionOption(sourceUnit, null);
            }
            else
            {
                var sourceMultiplier = sourceUnit.Multipliers.FirstOrDefault(multiplier => multiplier.Symbol.ToLower() == multiplierSymbol.ToLower());
                return new UnitSelectionOption(sourceUnit, sourceMultiplier);
            }
        }

        private List<UnitSelectionOption> GetQuantityUnits(Quantity quantity)
        {
            var unitOptions = new List<UnitSelectionOption>();

            foreach (var unit in quantity.Units)
            {
                unitOptions.Add(new UnitSelectionOption(unit, null));
                foreach (var multiplier in unit.Multipliers)
                {
                    unitOptions.Add(new UnitSelectionOption(unit, multiplier));
                }
            }

            return unitOptions;
        }

        internal UnitSystemStore InMemoryStore { get; private set; }
    }
}
