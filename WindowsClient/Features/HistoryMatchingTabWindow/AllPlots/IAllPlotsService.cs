using System.Collections.Generic;
using WindowsClient.Shared.UIModels.MaterialBalance;
using WindowsClient.Shared.Visualisation;
using WindowsClient.Shared.Visualisation.CartesianChart;

namespace WindowsClient.Features.HistoryMatchingTabWindow.AllPlots
{
    public interface IAllPlotsService
    {
        void PlotEnergyChart(List<ProductionEstimationResultRow> productionEstimationResult, IChartWrapper ChartControl, Tank _tank);
        void PlotGraphicalChart(List<ProductionEstimationResultRow> productionEstimationResult, IChartWrapper chart, Tank _tank);
        void PlotTehraniChart(RegressionResult newResult, IChartWrapper TehraniControl, Tank tank);
        void PlotWDChart(RegressionResult result, ICartesianChartControl ChartControl, Tank tank);
        void PlotPressure(ICartesianChartControl Chart, Tank tank, IList<PressureSimulationResultRow> result);
    }
}