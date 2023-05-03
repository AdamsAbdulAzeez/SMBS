using ChartUIControls.Controls;
using ChartUIControls.Controls.ChartData;
using ChartUIControls.Controls.Utils;
using Prism.Commands;
using System.Linq;
using WindowsClient.Shared.UIModels;
using WindowsClient.Shared.UIModels.MaterialBalance;
using WindowsClient.Shared.Utils;
using WindowsClient.Shared.Visualisation;
using WindowsClient.Shared.Visualisation.CartesianChart;

namespace WindowsClient.Features.HistoryMatchingTabWindow.PressureSimulationTab.Commands
{
    internal class PlotVariableCommand : DelegateCommandBase
    {
        public PlotVariableCommand(PressureSimulationViewModel viewModel, IChartWrapper chartControl)
        {
            _viewModel = viewModel;
            _chartControl = chartControl;
        }

        protected override bool CanExecute(object parameter) => true;

        protected override void Execute(object parameter)
        {
            _parameter = parameter;
            if (parameter is SimulationVariableModelType simulationVariableModelType)
            {
                PlotSimulationVariable(simulationVariableModelType);
            }
            if (parameter is HistoryVariableModelType historyVariableModelType)
            {
                PlotHistoryVariable(historyVariableModelType);
            }
        }

        private void PlotHistoryVariable(HistoryVariableModelType modelType)
        {
            TimeSeries series = new TimeSeries(true);
            switch (modelType.VariableName)
            {
                case HistoryVariableType.Pressure:
                    series = _viewModel.Tank.GetPressureData();
                    break;
                case HistoryVariableType.CummulativeOilProduction:
                    series = _viewModel.Tank.GetCummulativeOilProducedData();
                    break;
                case HistoryVariableType.CummulativeGasProduction:
                    series = _viewModel.Tank.GetCummulativeGasProducedData();
                    break;
                case HistoryVariableType.CummulativeWaterInjection:
                    series = _viewModel.Tank.GetCummulativeWaterInjectedData();
                    break;
                case HistoryVariableType.CummulativeGasInjection:
                    series = _viewModel.Tank.GetCummulativeGasInjectedData();
                    break;
                case HistoryVariableType.CummulativeWaterProduction:
                    series = _viewModel.Tank.GetCummulativeWaterProducedData();
                    break;
            }

            if (series.Count == 0) return;
            SetChartAxisAndPlot(modelType.VariableName.ToString(), series);
            modelType.IsEnabled = false;

            var sum = _viewModel.HistoryVariableTypes.Where(x => !x.IsEnabled).Count() + _viewModel.SimulationVariableTypes.Where(x => !x.IsEnabled).Count();
            if (sum >= 3)
            {
                _viewModel.HistoryVariableTypes.ToList().ForEach(x => { x.IsEnabled = false; });
                _viewModel.SimulationVariableTypes.ToList().ForEach(x => { x.IsEnabled = false; });
                return;
            }
        }

        private void SetChartAxisAndPlot(string historyVariableType, TimeSeries series)
        {
            if (PrimaryAxis == null)
            {
                _chartControl.Title = "Pressure Simulation";
                _chartControl.GetXAxis().AxisType = DataTypeEnum.Date;
                _chartControl.RefreshPlot();
                PrimaryAxis = new LineChartDataSeries();
                _chartControl.GetLeftYAxis(1).Title = historyVariableType.ToString();
                _chartControl.GetLeftYAxis(1).ExtendAxisRange = true;

                PrimaryAxis.YAxis = _chartControl.GetLeftYAxis(1).ID.ToString();
                ((YAxisNumberFormatter)_chartControl.GetLeftYAxis(1).AxisFormatter).DecimalPlaces = 10;
                SetChartSeries(historyVariableType.ToString(), PrimaryAxis);

                foreach (var item in series)
                {
                    PrimaryAxis.Data.Add(new DataPoint { XValue = item.Date, YValue = item.Value, Tooltip = $"X: {item.Date}   Y: {item.Value}" });
                }

                _chartControl.Data.Add(PrimaryAxis);
                _chartControl.itemSelector.AddItem(_chartControl as ChartWrapper);
                _chartControl.ReplotChart();
                return;
            }
            else if (PrimaryAxis != null && SecondaryAxis == null)
            {
                var check = historyVariableType;
                if (PrimaryAxis.SeriesName.Contains("Pressure") && historyVariableType.Contains("Pressure"))
                {
                    SecondaryAxis2 = new LineChartDataSeries();
                    _chartControl.GetLeftYAxis(1).Title = historyVariableType.ToString();

                    SecondaryAxis2.YAxis = _chartControl.GetLeftYAxis(1).ID.ToString();
                    ((YAxisNumberFormatter)_chartControl.GetLeftYAxis(1).AxisFormatter).DecimalPlaces = 10;
                    SetChartSeries(historyVariableType.ToString(), SecondaryAxis2);

                    foreach (var item in series)
                    {
                        SecondaryAxis2.Data.Add(new DataPoint { XValue = item.Date, YValue = item.Value, Tooltip = $"X: {item.Date}   Y: {item.Value}" });
                    }

                    _chartControl.AddDataSeries(new System.Collections.Generic.List<DataSeries> { SecondaryAxis2 });
                    _chartControl.itemSelector.AddItem(_chartControl as ChartWrapper);
                    _chartControl.ReplotChart();
                    return;
                }
                
                
                SecondaryAxis = new LineChartDataSeries();
                _chartControl.RightAxisCount = 1;
                _chartControl.GetRightYAxis(1).FontColor = ConverterUtils.StringToBrush("Black");
                _chartControl.GetRightYAxis(1).Title = historyVariableType.ToString();
                _chartControl.GetRightYAxis(1).ExtendAxisRange = true;

                SecondaryAxis.YAxis = _chartControl.GetRightYAxis(1).ID.ToString();
                ((YAxisNumberFormatter)_chartControl.GetRightYAxis(1).AxisFormatter).DecimalPlaces = 10;
                SetChartSeries(historyVariableType.ToString(), SecondaryAxis);

                foreach (var item in series)
                {
                    SecondaryAxis.Data.Add(new DataPoint { XValue = item.Date, YValue = item.Value, Tooltip = $"X: {item.Date}   Y: {item.Value}" });
                }

                _chartControl.AddDataSeries(new System.Collections.Generic.List<DataSeries> { SecondaryAxis });
                _chartControl.itemSelector.AddItem(_chartControl as ChartWrapper);
                _chartControl.ReplotChart();
                return;
            }
            else if (SecondaryAxis2 == null)
            {
                if (PrimaryAxis.SeriesName.Contains("Pressure") && historyVariableType.Contains("Pressure"))
                {
                    SecondaryAxis2 = new LineChartDataSeries();
                    _chartControl.GetLeftYAxis(1).Title = historyVariableType.ToString();

                    SecondaryAxis2.YAxis = _chartControl.GetLeftYAxis(1).ID.ToString();
                    ((YAxisNumberFormatter)_chartControl.GetLeftYAxis(1).AxisFormatter).DecimalPlaces = 10;
                    SetChartSeries(historyVariableType.ToString(), SecondaryAxis2);

                    foreach (var item in series)
                    {
                        SecondaryAxis2.Data.Add(new DataPoint { XValue = item.Date, YValue = item.Value, Tooltip = $"X: {item.Date}   Y: {item.Value}" });
                    }

                    _chartControl.AddDataSeries(new System.Collections.Generic.List<DataSeries> { SecondaryAxis2 });
                    _chartControl.itemSelector.AddItem(_chartControl as ChartWrapper);
                    _chartControl.ReplotChart();
                    return;
                }
                else if (SecondaryAxis.SeriesName.Contains("Pressure") && historyVariableType.Contains("Pressure"))
                {
                    SecondaryAxis2 = new LineChartDataSeries();
                    _chartControl.GetRightYAxis(1).Title = historyVariableType.ToString();

                    SecondaryAxis2.YAxis = _chartControl.GetRightYAxis(1).ID.ToString();
                    ((YAxisNumberFormatter)_chartControl.GetRightYAxis(1).AxisFormatter).DecimalPlaces = 10;
                    SetChartSeries(historyVariableType.ToString(), SecondaryAxis2);

                    foreach (var item in series)
                    {
                        SecondaryAxis2.Data.Add(new DataPoint { XValue = item.Date, YValue = item.Value, Tooltip = $"X: {item.Date}   Y: {item.Value}" });
                    }

                    _chartControl.AddDataSeries(new System.Collections.Generic.List<DataSeries> { SecondaryAxis2 });
                    _chartControl.itemSelector.AddItem(_chartControl as ChartWrapper);
                    _chartControl.ReplotChart();
                    return;
                }
            }
            else return;
        }

        private void SetChartSeries(string seriesName, LineChartDataSeries lineDataSeries)
        {
            lineDataSeries.LineSeries.Tag = lineDataSeries;
            lineDataSeries.YaxisDataType = DataTypeEnum.Linear;
            lineDataSeries.XaxisDataType = DataTypeEnum.Date;
            lineDataSeries.SeriesName = _parameter is SimulationVariableModelType ? $"Estimated {seriesName}" : $"Measured {seriesName}";
            lineDataSeries.SymbolColor = ConverterUtils.StringToBrush(PlotColor(seriesName));
            lineDataSeries.SymbolSize = seriesName == "Pressure" && _parameter is SimulationVariableModelType ? 0 : 5;
            lineDataSeries.LineColor = ConverterUtils.StringToBrush(PlotColor(seriesName));
            lineDataSeries.LinePattern = LinePatternEnum.Solid;
            lineDataSeries.LineThickness = seriesName == "Pressure" && _parameter is HistoryVariableModelType ? 0 : 3;
            lineDataSeries.LineType = LineTypeEnum.Spline;
            lineDataSeries.IsXAxisExtended = false;
        }

        private string PlotColor(string seriesName)
        {
            string color = "";
            if (_parameter is HistoryVariableModelType)
            {
                switch (seriesName)
                {
                    case "Pressure":
                        color = "Black";
                        break;
                    case "CummulativeOilProduction":
                        color = "Red";
                        break;
                    case "CummulativeGasProduction":
                        color = "Green";
                        break;
                    case "CummulativeWaterInjection":
                        color = "DarkBlue";
                        break;
                    case "CummulativeGasInjection":
                        color = "Green";
                        break;
                    case "CummulativeWaterProduction":
                        color = "Blue";
                        break;
                }
            }
            else
            {
                if (seriesName.ToLower().Contains("pressure"))
                    color = "Red";
                else if (seriesName.ToLower().Contains("influx"))
                    color = "CornflowerBlue";
                else if (seriesName.ToLower().Contains("water"))
                    color = "DarkBlue";
                else if (seriesName.ToLower().Contains("gas"))
                    color = "Cyan";
                else if (seriesName.ToLower().Contains("gor") || seriesName.ToLower().Contains("cgr"))
                    color = "Green";
                else if (seriesName.ToLower().Contains("oil") || seriesName.ToLower().Contains("liquid"))
                    color = "Red";
            }
            
            return color.ToString();
        }

        private void PlotSimulationVariable(SimulationVariableModelType modelType)
        {
            TimeSeries series = new TimeSeries(true);
            switch (modelType.VariableName)
            {
                case SimulationVariableType.Pressure:
                    series = _viewModel.Tank.PressureSimulationData.GetPressureData();
                    break;
                case SimulationVariableType.AquiferInflux:
                    series = _viewModel.Tank.PressureSimulationData.GetAquiferInfluxData();
                    break;
                case SimulationVariableType.AverageGasInjectionRate:
                    series = _viewModel.Tank.PressureSimulationData.GetAverageGasInjectionRateData();
                    break;
                case SimulationVariableType.AverageGasRate:
                    series = _viewModel.Tank.PressureSimulationData.GetAverageGasRateData();
                    break;
                case SimulationVariableType.AverageLiquidRate:
                    series = _viewModel.Tank.PressureSimulationData.GetAverageLiquidRateData();
                    break;
                case SimulationVariableType.AverageOilRate:
                    series = _viewModel.Tank.PressureSimulationData.GetAverageOilRateData();
                    break;
                case SimulationVariableType.AverageWaterInjectionRate:
                    series = _viewModel.Tank.PressureSimulationData.GetAverageWaterInjectionRateData();
                    break;
                case SimulationVariableType.AverageWaterRate:
                    series = _viewModel.Tank.PressureSimulationData.GetAverageWaterRateData();
                    break;
                case SimulationVariableType.CummulativeGasInjection:
                    series = _viewModel.Tank.PressureSimulationData.GetCummulativeGasInjectedData();
                    break;
                case SimulationVariableType.CummulativeGasProduction:
                    series = _viewModel.Tank.PressureSimulationData.GetCummulativeGasProducedData();
                    break;
                case SimulationVariableType.CummulativeOilProduction:
                    series = _viewModel.Tank.PressureSimulationData.GetCummulativeOilProducedData();
                    break;
                case SimulationVariableType.CummulativeWaterInjection:
                    series = _viewModel.Tank.PressureSimulationData.GetCummulativeWaterInjectedData();
                    break;
                case SimulationVariableType.OilDensity:
                    series = _viewModel.Tank.PressureSimulationData.GetOilDensityData();
                    break;
                case SimulationVariableType.OilFVF:
                    series = _viewModel.Tank.PressureSimulationData.GetOilFVFData();
                    break;
                case SimulationVariableType.OilRecoveryFactor:
                    series = _viewModel.Tank.PressureSimulationData.GetOilRecoveryFactorData();
                    break;
                case SimulationVariableType.OilViscosity:
                    series = _viewModel.Tank.PressureSimulationData.GetOilViscosityData();
                    break;
                case SimulationVariableType.ProducingCGR:
                    series = _viewModel.Tank.PressureSimulationData.GetProducingCGRData();
                    break;
                case SimulationVariableType.ProducingGOR:
                    series = _viewModel.Tank.PressureSimulationData.GetProducingGORData();
                    break;
                case SimulationVariableType.OilWaterContact:
                    series = _viewModel.Tank.PressureSimulationData.GetOilWaterContactData();
                    break;
                case SimulationVariableType.GasOilContact:
                    series = _viewModel.Tank.PressureSimulationData.GetGasOilContactData();
                    break;
            }
            series.ShowLine = true;
            series.LineWidth = 2;
            series.ShowMarker = true;

            if (series.Count == 0) return;
            SetChartAxisAndPlot(modelType.VariableName.ToString(), series);

            modelType.IsEnabled = false;

            var sum = _viewModel.HistoryVariableTypes.Where(x => !x.IsEnabled).Count() + _viewModel.SimulationVariableTypes.Where(x => !x.IsEnabled).Count();
            if (sum >= 3)
            {
                _viewModel.HistoryVariableTypes.ToList().ForEach(x => { x.IsEnabled = false; });
                _viewModel.SimulationVariableTypes.ToList().ForEach(x => { x.IsEnabled = false; });
                return;
            }
        }

        public void RefreshPlot()
        {
            _chartControl.RefreshPlot();
            PrimaryAxis = null;
            SecondaryAxis = null;
            SecondaryAxis2 = null;
            _viewModel.HistoryVariableTypes.ToList().ForEach(x => { x.IsEnabled = true; });
            _viewModel.SimulationVariableTypes.ToList().ForEach(x => { x.IsEnabled = true; });
        }

        private readonly PressureSimulationViewModel _viewModel;
        private readonly IChartWrapper _chartControl;
        
        private object _parameter;
        public LineChartDataSeries PrimaryAxis { get; set; }
        public LineChartDataSeries SecondaryAxis { get; set; }
        public LineChartDataSeries SecondaryAxis2 { get; set; }
    }
}
