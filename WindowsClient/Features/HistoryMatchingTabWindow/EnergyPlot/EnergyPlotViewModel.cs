using ChartUIControls.Controls;
using ChartUIControls.Controls.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WindowsClient.Shared.UIModels.MaterialBalance;
using WindowsClient.Shared.Utils;
using WindowsClient.Shared.Visualisation;

namespace WindowsClient.Features.HistoryMatchingTabWindow.WdPlot
{
    internal class EnergyPlotViewModel
    {
        public EnergyPlotViewModel(IChartWrapper chartControl, Tank tank)
        {
            ChartControl = chartControl;
            picasso = new Picasso(1);
            _tank = tank;
        }

        public void PlotChart(List<ProductionEstimationResultRow> productionEstimationResult)
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

        public string SplitJoinCamelCase(string source)
        {
            return string.Join(" ", Regex.Split(source, @"(?<!^)(?=[A-Z])"));
        }

        private readonly Tank _tank;
        private Picasso picasso;
        public IChartWrapper ChartControl { get; }
    }
}
