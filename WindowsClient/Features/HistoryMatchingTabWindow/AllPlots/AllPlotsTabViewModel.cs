using ChartUIControls.Controls;
using ChartUIControls.Controls.ChartData;
using ChartUIControls.Controls.Utils;
using WindowsClient.Features.HistoryMatchingTabWindow.TehraniAnalysisTab;
using WindowsClient.Shared.UIModels.MaterialBalance;
using WindowsClient.Shared.Visualisation.CartesianChart;
using WindowsClient.Shared.Visualisation;

namespace WindowsClient.Features.HistoryMatchingTabWindow.AllPlots
{
    internal class AllPlotsTabViewModel
    {
        private readonly Tank _tank;

        public AllPlotsTabViewModel(IChartWrapper chart, ICartesianChartControl chartControl, Tank tank)
        {
            allPlotsService = Ioc.Resolve<IAllPlotsService>();
            ChartControl = new CartesianChartControl();
            SimulationControl = new CartesianChartControl();
            TehraniControl = new ChartWrapper();
            GraphicalControl = new ChartWrapper();
            EnergyControl = new ChartWrapper();
            _tank = tank;
        }

        public void PlotCharts(RegressionResult newResult, System.Collections.Generic.List<ProductionEstimationResultRow> productionEstimationResult)
        {
            allPlotsService.PlotTehraniChart(newResult, TehraniControl, _tank);
            allPlotsService.PlotGraphicalChart(productionEstimationResult, GraphicalControl, _tank);
            allPlotsService.PlotEnergyChart(productionEstimationResult, EnergyControl, _tank);
            allPlotsService.PlotWDChart(newResult, ChartControl, _tank);
        }

        public void PlotSimulationChart(System.Collections.Generic.IList<PressureSimulationResultRow> result)
        {
            allPlotsService.PlotPressure(SimulationControl, _tank, result);
        }


        public ICartesianChartControl ChartControl { get; }
        public ICartesianChartControl SimulationControl { get; }
        public IChartWrapper TehraniControl { get; }
        public IChartWrapper GraphicalControl { get; }
        public IChartWrapper EnergyControl { get; }
        private IAllPlotsService allPlotsService;
    }
}
