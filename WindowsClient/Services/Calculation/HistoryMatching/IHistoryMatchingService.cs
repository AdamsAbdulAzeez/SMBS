using System.Collections.Generic;
using System.Threading.Tasks;
using WindowsClient.Shared.UIModels;
using WindowsClient.Shared.UIModels.MaterialBalance;
using WindowsClient.Shared.Visualisation;

namespace WindowsClient.Services.Calculator.HistoryMatching
{
    interface IHistoryMatchingService
    {
        Task<RegressionResult> SingleTankMatchAsync(Tank tank, List<RegressionVariable> regressionVariables, IChartWrapper chartControl);
        Task<EstimateResult> SingleTankEstimateAsync(Tank tank, IChartWrapper chartControl, XYDataSeries xyDataSeries = null, bool isTurnedOn = false);
    }
}
