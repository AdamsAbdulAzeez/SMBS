using System.Collections.Generic;
using System.Threading.Tasks;
using CypherCrescent.Units.Contracts;
using CypherCrescent.Units.Variables;

namespace CypherCrescent.Units.Services
{
    public interface IUnitService
    {
        Task InitialiseInFlatFileModeAsync(string appDirectory);
        void RegisterVariable(VariableBase variable);
        Task InitialiseFromSQLServer();
        Task InitialiseFromSQLite();
        IEnumerable<UnitSelectionOption> GetUnitOptions(string variableName);
        double? Convert(UnitSelectionOption from, UnitSelectionOption to, double? value);
        double? ToDatabaseUnit(UnitSelectionOption incomingUnit, double? value, string variableName);
        double? ToApplicationUnit(double dbValue, string variableName);
    }
}
