using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Linq;
using WindowsClient.ApplicationLayout.ToastNotification;
using WindowsClient.Features.HistoryMatchingTabWindow.FractionalFlowTab.Commands;
using WindowsClient.Services.Calculation;
using WindowsClient.Shared.UIModels;
using WindowsClient.Shared.UIModels.MaterialBalance;
using WindowsClient.Shared.Visualisation.CartesianChart;

namespace WindowsClient.Features.HistoryMatchingTabWindow.FractionalFlowTab
{
    internal class FractionalFlowTabViewModel : BindableBase
    {
        public FractionalFlowTabViewModel(Tank tank,
            Func<ICartesianChartControl> getChartControl,
            ICalculationServices calculationServices,
            IToastNotification toastNotification)
        {
            Tank = tank;
            Chart = getChartControl();
            SetDefaultChart();
            RegressCommand = new RegressCommand(this, calculationServices, toastNotification);
            ClearChartCommand = new DelegateCommand(() => { Chart.Clear(); SetDefaultChart(); });
            SwitchPlotCommand = new DelegateCommand(() => OnFractionalMatchResultChanged());
            Tank.FractionalMatchResultChanged += OnFractionalMatchResultChanged;
        }

        ~FractionalFlowTabViewModel()
        {

        }

        private void SetDefaultChart()
        {
            Chart.Clear();
            Chart.SetPrimaryYAxisTitle("Fractional Flow");
            Chart.SetPrimaryXRange(new Shared.Visualisation.CartesianChart.Range(0, 1));
            Chart.SetPrimaryYRange(new Shared.Visualisation.CartesianChart.Range(0, 1));
        }

        private void OnFractionalMatchResultChanged()
        {
            SetDefaultChart();
            switch (fracMatchingChoice)
            {
                case FracMatchingChoice.Fo:
                    SetOilFractionMatchResult();
                    break;
                case FracMatchingChoice.Fw:
                    SetWaterFractionMatchResult();
                    break;
                case FracMatchingChoice.Fg:
                    SetGasFractionMatchResult();
                    break;
            }
            
            Chart.Refresh();
        }

        private void SetWaterFractionMatchResult()
        {
            Chart.SetTitle("Fw Matching");
            Chart.SetPrimaryXAxisTitle("Water Saturation");

            var productionFractionsSeries = new XYDataSeries
            {
                Name = "Prod. Fw vs Prod. Sw",
                ShowLine = false,
                LineWidth = 2,
                MarkerSize = 8f,
                Color = "black"
            };

            FractionalMatchResultRow waterMatch = Tank.FractionalMatchResult.WaterMatch;
            var productionFractions = Enumerable.Zip(waterMatch.ProductionSx,
                waterMatch.ProductionFx);
            productionFractions.ToList().ForEach(row => productionFractionsSeries.Add((row.First, row.Second)));


            // plot simulated
            var simulatedFractionsSeries = new XYDataSeries
            {
                Name = "Fw vs Sw",
                ShowLine = true,
                LineWidth = 2,
                ShowMarker = false,
                Color = "blue"
            };

            var simulatedFractions = Enumerable.Zip(waterMatch.Sx,
                waterMatch.Fx);
            simulatedFractions.ToList().ForEach(row => simulatedFractionsSeries.Add((row.First, row.Second)));


            //water breakthrough
            var waterBreakthroughSeries = new XYDataSeries
            {
                Name = "Water breakthrough",
                ShowLine = true,
                LineWidth = 0.5f,
                ShowMarker = false,
                Color = "lightgreen"
            };

            if (!waterMatch.IsLineDataEmpty())
            {
                waterBreakthroughSeries.Add((waterMatch.LineSx[0], waterMatch.LineFx[0]));
                waterBreakthroughSeries.Add((waterMatch.LineSx[1], waterMatch.LineFx[1]));
            }

            Chart.AddSeries(new XYDataSeries[] { productionFractionsSeries, simulatedFractionsSeries, waterBreakthroughSeries });
        }

        private void SetGasFractionMatchResult()
        {
            Chart.SetTitle("Fg Matching");
            Chart.SetPrimaryXAxisTitle("Gas Saturation");

            var productionFractionsSeries = new XYDataSeries
            {
                Name = "Prod. Fg vs Prod. Sg",
                ShowLine = false,
                LineWidth = 2,
                MarkerSize = 8f,
                Color = "black"
            };

            FractionalMatchResultRow gasMatch = Tank.FractionalMatchResult.GasMatch;
            var productionFractions = Enumerable.Zip(gasMatch.ProductionSx,
                gasMatch.ProductionFx);
            productionFractions.ToList().ForEach(row => productionFractionsSeries.Add((row.First, row.Second)));


            // plot simulated
            var simulatedFractionsSeries = new XYDataSeries
            {
                Name = "Fg vs Sg",
                ShowLine = true,
                LineWidth = 2,
                ShowMarker = false,
                Color = "blue"
            };

            var simulatedFractions = Enumerable.Zip(gasMatch.Sx,
                gasMatch.Fx);
            simulatedFractions.ToList().ForEach(row => simulatedFractionsSeries.Add((row.First, row.Second)));


            //water breakthrough
            var waterBreakthroughSeries = new XYDataSeries
            {
                Name = "Water breakthrough",
                ShowLine = true,
                LineWidth = 0.5f,
                ShowMarker = false,
                Color = "lightgreen"
            };


            if (!gasMatch.IsLineDataEmpty())
            {
                waterBreakthroughSeries.Add((gasMatch.LineSx[0], gasMatch.LineFx[0]));
                waterBreakthroughSeries.Add((gasMatch.LineSx[1], gasMatch.LineFx[1]));
            }
            
            Chart.AddSeries(new XYDataSeries[] { productionFractionsSeries, simulatedFractionsSeries, waterBreakthroughSeries });
        }

        private void SetOilFractionMatchResult()
        {
            Chart.SetTitle("Fo Matching");
            Chart.SetPrimaryXAxisTitle("Oil Saturation");

            var productionFractionsSeries = new XYDataSeries
            {
                Name = "Prod. Fo vs Prod. So",
                ShowLine = false,
                LineWidth = 2,
                MarkerSize = 8f,
                Color = "black"
            };

            FractionalMatchResultRow oilMatch = Tank.FractionalMatchResult.OilMatch;

            var productionFractions = Enumerable.Zip(oilMatch.ProductionSx,
                oilMatch.ProductionFx);
            productionFractions.ToList().ForEach(row => productionFractionsSeries.Add((row.First, row.Second)));


            // plot simulated
            var simulatedFractionsSeries = new XYDataSeries
            {
                Name = "Fo vs So",
                ShowLine = true,
                LineWidth = 2,
                ShowMarker = false,
                Color = "blue"
            };

            var simulatedFractions = Enumerable.Zip(oilMatch.Sx,
                oilMatch.Fx);
            simulatedFractions.ToList().ForEach(row => simulatedFractionsSeries.Add((row.First, row.Second)));


            //water breakthrough
            var waterBreakthroughSeries = new XYDataSeries
            {
                Name = "Water breakthrough",
                ShowLine = true,
                LineWidth = 0.5f,
                ShowMarker = false,
                Color = "lightgreen"
            };

            if (!oilMatch.IsLineDataEmpty())
            {
                waterBreakthroughSeries.Add((oilMatch.LineSx[0], oilMatch.LineFx[0]));
                waterBreakthroughSeries.Add((oilMatch.LineSx[1], oilMatch.LineFx[1]));
            }

            Chart.AddSeries(new XYDataSeries[] { productionFractionsSeries, simulatedFractionsSeries, waterBreakthroughSeries });
        }

        public Tank Tank { get; }
        public RegressCommand RegressCommand { get; }
        public DelegateCommand ClearChartCommand { get; }
        public DelegateCommand SwitchPlotCommand { get; }

        private FracMatchingChoice fracMatchingChoice;
        public FracMatchingChoice FracMatchingChoice
        {
            get => fracMatchingChoice;
            set { fracMatchingChoice = value; RaisePropertyChanged(); }
        }

        public ICartesianChartControl Chart { get; }
    }
}
