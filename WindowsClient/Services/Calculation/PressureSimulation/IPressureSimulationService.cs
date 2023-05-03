using System.Collections.Generic;
using System.Threading.Tasks;
using WindowsClient.Shared.UIModels.MaterialBalance;

namespace WindowsClient.Services.Calculator.PressureSimulation
{
    interface IPressureSimulationService
    {
        Task<List<PressureSimulationResultRow>> SimulateAsync(Tank tank, DateUpdate dateUpdate, int step);
    }
}
