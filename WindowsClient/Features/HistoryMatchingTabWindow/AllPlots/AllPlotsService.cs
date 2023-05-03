using ChartUIControls.Controls;
using ChartUIControls.Controls.ChartData;
using ChartUIControls.Controls.Legend;
using ChartUIControls.Controls.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WindowsClient.Features.HistoryMatchingTabWindow.TehraniAnalysisTab;
using WindowsClient.Features.HistoryMatchingTabWindow.WdPlot;
using WindowsClient.Shared.UIModels.MaterialBalance;
using WindowsClient.Shared.Visualisation.CartesianChart;
using WindowsClient.Shared.Visualisation;
using WindowsClient.Shared.UIModels;
using WindowsClient.Shared.Utils;
using System.Windows.Media;

namespace WindowsClient.Features.HistoryMatchingTabWindow.AllPlots
{
    public class AllPlotsService : IAllPlotsService
    {
        public AllPlotsService()
        {
            picasso = new Picasso(1);
        }

        #region Tehrani Plot
        public void PlotTehraniChart(RegressionResult newResult, IChartWrapper TehraniControl, Tank tank)
        {
            if (newResult == null) return;

            TehraniControl.RefreshPlot();
            PlotMeasuredNp(TehraniControl, tank);
            newResult.EstimatedNpSeries.Color = "firebrick";
            newResult.EstimatedNpSeries.Name = "Estimated Np";
            newResult.EstimatedNpSeries.ShowLine = true;
            newResult.EstimatedNpSeries.LineWidth = 3;

            LineChartDataSeries lineDataSeries = new LineChartDataSeries();
            lineDataSeries.LineSeries.Tag = lineDataSeries;
            lineDataSeries.SeriesName = "Estimated Np";
            lineDataSeries.SymbolColor = ConverterUtils.StringToBrush("firebrick");
            lineDataSeries.LineColor = ConverterUtils.StringToBrush("firebrick");
            lineDataSeries.SymbolSize = 0;
            lineDataSeries.LineThickness = 2;

            foreach (var item in newResult.EstimatedNpSeries)
            {
                lineDataSeries.Data.Add(new DataPoint { XValue = item.X, YValue = item.Y, Tooltip = $"X: {item.X}   Y: {item.Y}" });
            }

            TehraniControl.Data.Add(lineDataSeries);
            TehraniControl.itemSelector.AddItem(TehraniControl as ChartWrapper);
            //ChartCont
            PlotSelectedPoints1(newResult, TehraniControl);
        }

        private void PlotMeasuredNp(IChartWrapper TehraniControl, Tank _tank)
        {
            TehraniControl.Title = $"Tehrani Analysis Plot for {_tank.Name}";
            TehraniControl.GetLeftYAxis(1).Title = "Pressure (Psia)";
            TehraniControl.GetXAxis().Title = "Cumulative Oil Production (MMSTB)";
            //sTehraniControl.LegendPosition = LegendPositionEnum.None;
            TehraniControl.GetXAxis().ExtendAxisRange = false;
            var historyPlot = _tank.GetPressureVsCummulativeProduction();
            //TehraniControl.GetXAxis().AutomaticTickGeneration = false;
            //TehraniControl.GetXAxis().Xmin = 0;
            //TehraniControl.GetXAxis().XTick = 5;
            //TehraniControl.GetXAxis().Xmax = historyPlot.Select(x => x.X).Max() + 10;
            historyPlot.Color = "Blue";
            historyPlot.Name = "Measured Np";

            LineChartDataSeries lineDataSeries = new LineChartDataSeries();
            lineDataSeries.LineSeries.Tag = lineDataSeries;
            lineDataSeries.SeriesName = "Measured Np";
            lineDataSeries.SymbolColor = ConverterUtils.StringToBrush("Blue");
            lineDataSeries.SymbolSize = 5;
            lineDataSeries.LineColor = ConverterUtils.StringToBrush("Blue");
            lineDataSeries.LinePattern = LinePatternEnum.Solid;
            lineDataSeries.LineThickness = 0;
            lineDataSeries.LineType = LineTypeEnum.StairStep;

            foreach (var item in historyPlot)
            {
                lineDataSeries.Data.Add(new DataPoint { XValue = item.X, YValue = item.Y, Tooltip = $"X: {item.X}   Y: {item.Y}" });
            }

            TehraniControl.Data.Add(lineDataSeries);
            TehraniControl.itemSelector.AddItem(TehraniControl as ChartWrapper);
        }

        private void PlotSelectedPoints1(RegressionResult newResult, IChartWrapper TehraniControl)
        {
            if (newResult.TurnOffPoints == null || newResult.TurnOffPoints.Count == 0)
            {
                if (newResult.SelectedDataPoint.Count != 0)
                {
                    newResult.TurnOffPoints = new XYDataSeries(true, true);
                    foreach (var item in newResult.SelectedDataPoint)
                        newResult.TurnOffPoints.Add(((double)item.XValue, (double)item.YValue));
                }
                else return;
            }
            TehraniControl.GetXAxis().AxisType = DataTypeEnum.Linear;
            TehraniControl.SelectedRegressionResult.SelectedPoints = new ScatterChartDataSeries();
            TehraniControl.SelectedRegressionResult.SelectedPoints.YAxis = TehraniControl.GetLeftYAxis(1).ID.ToString();
            TehraniControl.SelectedRegressionResult.SelectedPoints.YaxisDataType = DataTypeEnum.Logarithmic;
            TehraniControl.SelectedRegressionResult.SelectedPoints.SymbolSize = 5;
            TehraniControl.SelectedRegressionResult.SelectedPoints.FillPattern = FillPatternEnum.Solid;
            TehraniControl.SelectedRegressionResult.SelectedPoints.SymbolColor = ConverterUtils.StringToBrush("LightGray");
            TehraniControl.SelectedRegressionResult.SelectedPoints.IsXAxisExtended = false;
            TehraniControl.SelectedRegressionResult.SelectedPoints.SeriesName = "Selected Point";

            TehraniControl.SelectedRegressionResult.SelectedPoints.GetLegendItem().Visibility = ConverterUtils.GetVisibility("Collapsed");

            //----------------------------//
            TehraniControl.SelectedRegressionResult.SelectedDataPoint.Clear();
            //----------------------------//

            for (int i = 0; i < newResult.TurnOffPoints.Count; i++)
            {
                DataPoint thisDataPoint = new DataPoint();
                thisDataPoint.XValue = newResult.TurnOffPoints[i].X;
                thisDataPoint.YValue = newResult.TurnOffPoints[i].Y;
                thisDataPoint.Tooltip = $"X: {newResult.TurnOffPoints[i].X}   Y: {newResult.TurnOffPoints[i].Y}";
                TehraniControl.SelectedRegressionResult.SelectedPoints.Data.Add(thisDataPoint);

                //Added this
                //----------------------------//
                DataPointModel2 modelPoint = new DataPointModel2();
                modelPoint.XValue = newResult.TurnOffPoints[i].X;
                modelPoint.YValue = newResult.TurnOffPoints[i].Y;
                modelPoint.ToolTip = $"X: {newResult.TurnOffPoints[i].X}   Y: {newResult.TurnOffPoints[i].Y}";
                TehraniControl.SelectedRegressionResult.SelectedDataPoint.Add(modelPoint);
                //----------------------------//
            }

            TehraniControl.Data.Add(TehraniControl.SelectedRegressionResult.SelectedPoints);

            TehraniControl.itemSelector.AddItem(TehraniControl as ChartWrapper);
        }
        #endregion


        #region Graphical Plot
        public void PlotGraphicalChart(List<ProductionEstimationResultRow> productionEstimationResult, IChartWrapper chart, Tank _tank)
        {
            Tank = _tank;
            var stoiipOrGiip = "";

            switch (_tank.FlowingFluid)
            {
                case FluidType.Gas:
                case FluidType.Condensate:
                    FoverEt = productionEstimationResult.Select(CalcFOverET).ToList();
                    WeOverEt = productionEstimationResult.Select(CalcWeOverET).ToList();

                    stoiipOrGiip = $"GIIP = {_tank.GIIP.CurrentValue.DisplayValue} {_tank.GIIP.CurrentValue.DisplayUnit}";
                    break;

                case FluidType.Oil:
                    FoverEt = productionEstimationResult.Select(CalcFOverET).ToList();
                    WeOverEt = productionEstimationResult.Select(CalcWeOverET).ToList();

                    stoiipOrGiip = $"STOIIP = {_tank.STOIP.CurrentValue.DisplayValue} {_tank.STOIP.CurrentValue.DisplayUnit}";
                    break;

            }


            double c = _tank.FlowingFluid == FluidType.Oil ?
                _tank.STOIP.CurrentValue.DisplayValue.Value :
                _tank.GIIP.CurrentValue.DisplayValue.Value;
            FoverEt = FoverEt.Where(foe => foe != 0).ToList();
            WeOverEt = WeOverEt.Where(woe => woe != 0).ToList();
            MathematicsLibrary.ColVec y = FoverEt.Select(yy => yy - c).ToList();
            MathematicsLibrary.ColVec x = WeOverEt;
            double m = y.Transpose() * y / (x.Transpose() * x);
            MathematicsLibrary.ColVec result = new List<double> { m, c };
            XLineOverEt = MathematicsLibrary.ColVec.Linspace(0, WeOverEt.Max(), 100).ToList();
            YLineOverEt = MathematicsLibrary.Optimizers.Trend(XLineOverEt, result, "Linear").ToList();

            PlotChart(stoiipOrGiip, chart);
        }

        private double CalcWeOverET(ProductionEstimationResultRow x)
        {
            var unit = Tank.FlowingFluid == FluidType.Oil ? "MMstb" : "MMscf";
            XTitle = $"We/Et {unit};We/Et";
            return x.Et == 0 ? 0 : x.AquiferInflux / x.Et;
        }

        private double CalcFOverET(ProductionEstimationResultRow x)
        {
            var unit = Tank.FlowingFluid == FluidType.Oil ? "MMstb" : "MMscf";
            YTitle = $"F/Et {unit};F/Et";
            return x.Et == 0 ? 0 : x.F / x.Et;
        }

        private void PlotChart(string stoiipOrGiip, IChartWrapper ChartControl)
        {
            ChartControl.Title = $"{Tank.Name} Graphical Plot";
            //ChartControl.LegendPosition = LegendPositionEnum.None;

            ChartControl.RefreshPlot();
            ChartControl.PlotLineAction(WeOverEt, FoverEt, "F/Et vs We/Et ", XTitle.Split(';')[0], YTitle.Split(';')[0], ConverterUtils.StringToBrush("Red"), 0, 5, XTitle.Split(';')[1], YTitle.Split(';')[1]);
            ChartControl.PlotLineAction(XLineOverEt, YLineOverEt, stoiipOrGiip, XTitle.Split(';')[0], YTitle.Split(';')[0], ConverterUtils.StringToBrush("Blue"), 2, 0, XTitle.Split(';')[1], YTitle.Split(';')[1]);
            ChartControl.ReplotChart();
        }

        private string XTitle { get; set; }
        private string YTitle { get; set; }
        List<double> FoverEt { get; set; } = new List<double>();
        List<double> WeOverEt { get; set; } = new List<double>();
        List<double> YLineOverEt { get; set; } = new List<double>();
        List<double> XLineOverEt { get; set; } = new List<double>();
        private Tank Tank;
        #endregion

        #region Energy Plot
        public void PlotEnergyChart(List<ProductionEstimationResultRow> productionEstimationResult, IChartWrapper ChartControl, Tank _tank)
        {
            ChartControl.RefreshPlot();
            ChartControl.Title = $"{_tank.Name} Drive Mechanism";
            ChartControl.GetLeftYAxis(1).Title = "Drive Index";
            ChartControl.GetXAxis().Title = "Production Date";
            ChartControl.GetXAxis().AxisOrientation = AxisOrientation.DeviateLeft;
            ChartControl.GetXAxis().AxisType = DataTypeEnum.Date;

            if (productionEstimationResult.Count == 0) return;
            var productionDate = productionEstimationResult.Skip(1).Select(x => x.Time).ToList();

            var xAxis = ChartControl.GetXAxis();
            xAxis.AutomaticTickGeneration = false;
            xAxis.ExtendAxisRange = false;
            xAxis.Xmin = DuplicateEnergyPointProductionDate(productionDate).Min().Ticks;
            xAxis.Xmax = DuplicateEnergyPointProductionDate(productionDate).Max().Ticks;
            xAxis.UpdateXAxis();

            ChartControl.GetLeftYAxis(1).IsStackingEnabled = true;

            ChartControl.GetLeftYAxis(1).AutomaticTickGeneration = false;
            ChartControl.GetLeftYAxis(1).Ymax = 1;
            ChartControl.GetLeftYAxis(1).Ymin = 0;
            ChartControl.GetLeftYAxis(1).YTick = 0.2;

            var query = ExtractDriveIndices(productionEstimationResult);
            foreach (var index in query)
            {
                ChartControl.PlotEnergyAction(DuplicateEnergyPointProductionDate(productionDate)
                    , DuplicateEnergyPointIndexValue(index.IndexValues), index.SeriesName, "Production Date", "Drive Index", index.color);
            }

            ChartHelper.GetInstance().UpdateAxes(ChartControl as ChartWrapper, ChartControl.GetXAxis().AxisType);
            ChartControl.ReplotChart();
        }

        private List<DateTime> DuplicateEnergyPointProductionDate(List<DateTime> productionDate)
        {
            if (productionDate.Count == 1)
            {
                productionDate.Add(productionDate[0].AddYears(1));
            }
            return productionDate;
        }

        private List<double> DuplicateEnergyPointIndexValue(List<double> indexValues)
        {
            if (indexValues.Count == 1)
            {
                indexValues.Add(indexValues[0]);
            }
            return indexValues;
        }

        private List<ReservoirDriveIndex> ExtractDriveIndices(List<ProductionEstimationResultRow> estimationResult)
        {
            var reservoirDrives = new List<ReservoirDriveIndex>();

            var productionEstimationResult = estimationResult.Skip(1);

            var PVCompressEnergy = productionEstimationResult.Select(x => x.PVCompressEnergy);
            if (PVCompressEnergy.Any(x => x > 0 && x <= 1))
                reservoirDrives.Add(GetDriveIndex(PVCompressEnergy, nameof(PVCompressEnergy)));

            var GasCapExpansionEnergy = productionEstimationResult.Select(x => x.GasCapExpansionEnergy);
            if (GasCapExpansionEnergy.Any(x => x > 0 && x <= 1))
                reservoirDrives.Add(GetDriveIndex(GasCapExpansionEnergy, nameof(GasCapExpansionEnergy)));

            var FluidExpansionEnergy = productionEstimationResult.Select(x => x.FluidExpansionEnergy);
            if (FluidExpansionEnergy.Any(x => x > 0 && x <= 1))
                reservoirDrives.Add(GetDriveIndex(FluidExpansionEnergy, nameof(FluidExpansionEnergy)));

            var WaterInjectedEnergy = productionEstimationResult.Select(x => x.WaterInjectionEnergy);
            if (WaterInjectedEnergy.Any(x => x > 0 && x <= 1))
                reservoirDrives.Add(GetDriveIndex(WaterInjectedEnergy, nameof(WaterInjectedEnergy)));

            var GasInjectedEnergy = productionEstimationResult.Select(x => x.GasInjectionEnergy);
            if (GasInjectedEnergy.Any(x => x > 0 && x <= 1))
                reservoirDrives.Add(GetDriveIndex(GasInjectedEnergy, nameof(GasInjectedEnergy)));

            var CompactionConnateWaterEnergy = productionEstimationResult.Select(x => x.CompactionConnateWaterDriveEnergy);
            if (CompactionConnateWaterEnergy.Any(x => x > 0 && x <= 1))
                reservoirDrives.Add(GetDriveIndex(CompactionConnateWaterEnergy, nameof(CompactionConnateWaterEnergy)));

            var WaterInfluxEnergy = productionEstimationResult.Select(x => x.WaterInfluxEnergy);
            if (WaterInfluxEnergy.Any(x => x > 0 && x <= 1))
                reservoirDrives.Add(GetDriveIndex(WaterInfluxEnergy, nameof(WaterInfluxEnergy)));

            var SolutionGasDriveEnergy = productionEstimationResult.Select(x => x.SolutionGasDriveEnergy);
            if (SolutionGasDriveEnergy.Any(x => x > 0 && x <= 1))
                reservoirDrives.Add(GetDriveIndex(SolutionGasDriveEnergy, nameof(SolutionGasDriveEnergy)));

            var query = reservoirDrives.OrderBy(x => x.IndexValues.Max()).ToList();
            return query;
        }

        private ReservoirDriveIndex GetDriveIndex(IEnumerable<double> values, string variableName)
        {
            var color = picasso.GetBrush(2);
            if (variableName.ToLower().Contains("influx"))
                color = ConverterUtils.StringToBrush("CornflowerBlue");
            else if (variableName.ToLower().Contains("water"))
                color = ConverterUtils.StringToBrush("DarkBlue");
            else if (variableName.ToLower().Contains("gas"))
                color = ConverterUtils.StringToBrush("Cyan");
            else if (variableName.ToLower().Contains("fluid"))
                color = ConverterUtils.StringToBrush("Red");


            return new ReservoirDriveIndex
            {
                color = color,
                SeriesName = SplitJoinCamelCase(variableName),
                IndexValues = values.ToList(),
            };
        }

        private string SplitJoinCamelCase(string source)
        {
            return string.Join(" ", Regex.Split(source, @"(?<!^)(?=[A-Z])"));
        }
        #endregion

        #region WD Plot
        public void PlotWDChart(RegressionResult result, ICartesianChartControl ChartControl, Tank _tank)
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
            ChartControl.SetPrimaryXRange(new Shared.Visualisation.CartesianChart.Range(1, null));
        }
        #endregion

        #region Simulation Plot
        public void PlotPressure(ICartesianChartControl Chart, Tank tank, IList<PressureSimulationResultRow> result)
        {
            Chart.Clear();
            Chart.SetTitle($"Pressure Simulation Plot for {tank.Name}");
            Chart.SetPrimaryXAxisTitle("Date");
            Chart.SetPrimaryYAxisTitle("Pressure");

            var measuredPressurePlotData = tank.GetSelectedBhpData();
            measuredPressurePlotData.Color = "Blue";
            measuredPressurePlotData.Name = "Measured Pressure";
            measuredPressurePlotData.ShowLine = false;
            measuredPressurePlotData.LineWidth = 5;
            measuredPressurePlotData.ShowMarker = true;

            var simulatedPressurePlotData = GetSimulatedPressureData(result);
            simulatedPressurePlotData.Color = "Red";
            simulatedPressurePlotData.Name = "Simulated Pressure";
            simulatedPressurePlotData.ShowLine = true;
            simulatedPressurePlotData.LineWidth = 5;
            simulatedPressurePlotData.ShowMarker = true;

            Chart.AddSeries(new[] { measuredPressurePlotData, simulatedPressurePlotData });
        }

        private TimeSeries GetSimulatedPressureData(IList<PressureSimulationResultRow> result)
        {
            var series = new TimeSeries(true);
            result.ToList()
                    .ForEach(row => series.Add((row.Time, row.TankPressure)));
            series.Name = "Sim. Pressure";
            return series;
        }
        #endregion

        private Picasso picasso;
    }
}
