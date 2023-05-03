using ChartUIControls.Controls;
using ChartUIControls.Controls.ChartData;
using ChartUIControls.Controls.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using WindowsClient.Services.Calculation;
using WindowsClient.Shared.UIModels;
using WindowsClient.Shared.UIModels.MaterialBalance;
using WindowsClient.Shared.Visualisation;
using WindowsClient.Shared.Visualisation.CartesianChart;
using Range = WindowsClient.Shared.Visualisation.CartesianChart.Range;

namespace WindowsClient.Features.HistoryMatchingTabWindow.WdPlot
{
    internal class WdPlotTabViewModel
    {
        public WdPlotTabViewModel(ICartesianChartControl chartControl, Tank tank)
        {
            ChartControl = chartControl;
            _tank = tank;
            calculator = Ioc.Resolve<ICalculationServices>();
            ChartControl.SetTitle($"{tank.Name} WD Plot");
            ChartControl.SetPrimaryXAxisTitle("TD");
            ChartControl.SetPrimaryYAxisTitle("WD");

            ChartControl.SetLogarithmicXAxis();
        }

        public void Refresh(RegressionResult result)
        {
            ChartControl.Clear();
            ChartControl.SetTitle($"{_tank.Name} WD Plot");
            ChartControl.SetPrimaryXAxisTitle("TD");
            ChartControl.SetPrimaryYAxisTitle("WD");

            ChartControl.SetLogarithmicXAxis();
            result.TdSeriesForReservoirRadiusSeries.Name = result.NewWdSeriesAtOtherRadi.Last().Name;
            result.TdSeriesForReservoirRadiusSeries.Color = "red";
            result.TdSeriesForReservoirRadiusSeries.ShowMarker = false;
            result.TdSeriesForReservoirRadiusSeries.LineWidth = 1;
            result.TdSeriesForReservoirRadiusSeries.ShowLine = true;

            result.SelectedTdSeries.Color = "blue";
            result.SelectedTdSeries.ShowLine = false;
            result.SelectedTdSeries.LineWidth = 0;
            result.SelectedTdSeries.MarkerSize = 10F;

            List<XYDataSeries> wdSeriesAtOtherRadi = new List<XYDataSeries>();
            //result.WdSeriesAtOtherRadi.ForEach(series =>
            //{
            //    series.Color = "green";
            //    series.ShowLine = true;
            //    series.ShowMarker = false;
            //    series.LineWidth = 1;
            //});
            var wdSeriesRadi = result.NewWdSeriesAtOtherRadi.Take(result.NewWdSeriesAtOtherRadi.Count - 1).ToList();
            for (int i = 0; i < wdSeriesRadi.Count; i++)
            {
                wdSeriesRadi[i].WdAtOtherRadi.Name = result.NewWdSeriesAtOtherRadi[i].Name;
                wdSeriesRadi[i].WdAtOtherRadi.Color = "green";
                wdSeriesRadi[i].WdAtOtherRadi.ShowLine = true;
                wdSeriesRadi[i].WdAtOtherRadi.ShowMarker = false;
                wdSeriesRadi[i].WdAtOtherRadi.LineWidth = 1;
                wdSeriesAtOtherRadi.Add(wdSeriesRadi[i].WdAtOtherRadi);
            }

            ChartControl.AddSeries(new[] { result.SelectedTdSeries, result.TdSeriesForReservoirRadiusSeries }.Concat(wdSeriesAtOtherRadi));
            ChartControl.SetPrimaryXRange(new Range(1, null));
        }

        public ICartesianChartControl ChartControl { get; }
        private readonly Tank _tank;
        private ICalculationServices calculator;
    }
}
