using ChartUIControls.Controls;
using ChartUIControls.Controls.ChartData;
using ChartUIControls.Controls.Utils;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WindowsClient.ApplicationLayout.ToastNotification;
using WindowsClient.Features.TankInputTabWindow.ProductionDataTab.Commands;
using WindowsClient.Services.ExcelImport;
using WindowsClient.Shared.UIModels;
using WindowsClient.Shared.UIModels.MaterialBalance;
using WindowsClient.Shared.Utils;
using WindowsClient.Shared.Visualisation;
using WindowsClient.Shared.Visualisation.CartesianChart;

namespace WindowsClient.Features.TankInputTabWindow.ProductionDataTab
{
    internal class ProductionDataTabViewModel : BindableBase
    {
        public ProductionDataTabViewModel(Tank tank,
            IToastNotification toastNotification,
            IMaterialBalanceDataReader materialBalanceDataReader,
            Func<IChartWrapper> getChartControl)
        {
            Tank = tank;
            SetProductionData();
            ChartControl = getChartControl();
            PasteCommand = new PasteCommand(toastNotification);
            ClearCommand = new ClearCommand();
            ShowTableCommand = new DelegateCommand(() => ShowTable = true, () => !ShowTable);
            ShowPlotCommand = new DelegateCommand(() => ShowTable = false, () => ShowTable);
            InitializeVariables();
            PlotVariableCommand = new DelegateCommand<object>(PlotVariableAction);
            RefreshPlotCommand = new DelegateCommand(RefreshPlotAction);
            ShowTable = true;
            ImportProductionDataCommand = new ImportProductionDataCommand(materialBalanceDataReader, toastNotification);
            ProductionDataChanged += () => SetProductionData();
        }

        private void RefreshPlotAction()
        {
            ChartControl.RefreshPlot();
            PrimaryAxis = null;
            SecondaryAxis = null;
            HistoryVariableTypes.ToList().ForEach(x => { x.IsEnabled = true; });
        }

        private void InitializeVariables()
        {
            HistoryVariableTypes = new ObservableCollection<HistoryVariableModelType>()
            {
                new HistoryVariableModelType() { VariableName = HistoryVariableType.Pressure, IsEnabled = true },
                new HistoryVariableModelType() { VariableName = HistoryVariableType.CummulativeOilProduction, IsEnabled = true },
                new HistoryVariableModelType() { VariableName = HistoryVariableType.CummulativeGasProduction, IsEnabled = true },
                new HistoryVariableModelType() { VariableName = HistoryVariableType.CummulativeWaterInjection, IsEnabled = true },
                new HistoryVariableModelType() { VariableName = HistoryVariableType.CummulativeGasInjection, IsEnabled = true },
                new HistoryVariableModelType() { VariableName = HistoryVariableType.CummulativeWaterProduction, IsEnabled = true },

            };
        }

        private void PlotVariableAction(object obj)
        {
            if (obj is HistoryVariableModelType historyVariableModelType)
            {
                GetSimulatedPressureData(historyVariableModelType);
            }
        }

        private void SetProductionData()
        {
            Task.Run(() =>
            {
                ProductionData = new(Tank.ProductionData);
            });
        }

        private void PlotChart(HistoryVariableType variableName, TimeSeries series)
        {
            if (PrimaryAxis == null)
            {
                ChartControl.Title = $"Production Data Plot for {Tank.Name}";
                ChartControl.GetXAxis().AxisType = DataTypeEnum.Date;
                ChartControl.RefreshPlot();
                PrimaryAxis = new LineChartDataSeries();
                ChartControl.GetLeftYAxis(1).Title = variableName.ToString();
                ChartControl.GetLeftYAxis(1).ExtendAxisRange = true;

                PrimaryAxis.YAxis = ChartControl.GetLeftYAxis(1).ID.ToString();
                ((YAxisNumberFormatter)ChartControl.GetLeftYAxis(1).AxisFormatter).DecimalPlaces = 10;
                SetAxisData(variableName.ToString(), PrimaryAxis);

                foreach (var item in series)
                {
                    PrimaryAxis.Data.Add(new DataPoint { XValue = item.Date, YValue = item.Value, Tooltip = $"X: {item.Date}   Y: {item.Value}" });
                }

                ChartControl.Data.Add(PrimaryAxis);
                ChartControl.itemSelector.AddItem(ChartControl as ChartWrapper);
                ChartControl.ReplotChart();
            }
            else
            {
                SecondaryAxis = new LineChartDataSeries();
                ChartControl.RightAxisCount = 1;
                ChartControl.GetRightYAxis(1).FontColor = ConverterUtils.StringToBrush("Black");
                ChartControl.GetRightYAxis(1).Title = variableName.ToString();
                ChartControl.GetRightYAxis(1).ExtendAxisRange = true;

                SecondaryAxis.YAxis = ChartControl.GetRightYAxis(1).ID.ToString();
                ((YAxisNumberFormatter)ChartControl.GetRightYAxis(1).AxisFormatter).DecimalPlaces = 10;
                SetAxisData(variableName.ToString(), SecondaryAxis);

                foreach (var item in series)
                {
                    SecondaryAxis.Data.Add(new DataPoint { XValue = item.Date, YValue = item.Value, Tooltip = $"X: {item.Date}   Y: {item.Value}" });
                }

                ChartControl.AddDataSeries(new List<DataSeries> { SecondaryAxis });
                ChartControl.itemSelector.AddItem(ChartControl as ChartWrapper);
                ChartControl.ReplotChart();
            }
        }

        private void SetAxisData(string seriesName, LineChartDataSeries lineDataSeries)
        {
            lineDataSeries.LineSeries.Tag = lineDataSeries;
            lineDataSeries.YaxisDataType = DataTypeEnum.Linear;
            lineDataSeries.XaxisDataType = DataTypeEnum.Date;
            lineDataSeries.SeriesName = seriesName;
            lineDataSeries.SymbolColor = ConverterUtils.StringToBrush(PlotColor(seriesName));
            lineDataSeries.SymbolSize = 5;
            lineDataSeries.LineColor = ConverterUtils.StringToBrush(PlotColor(seriesName));
            lineDataSeries.LinePattern = LinePatternEnum.Solid;
            lineDataSeries.LineThickness = seriesName == "Pressure" ? 0 : 3;
            lineDataSeries.LineType = LineTypeEnum.Spline;
            lineDataSeries.IsXAxisExtended = false;
        }

        private string PlotColor(string seriesName)
        {
            string color = "";
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
            return color;
        }
        public void GetSimulatedPressureData(HistoryVariableModelType modelType)
        {
            var series = new TimeSeries(true);
            switch (modelType.VariableName)
            {
                case HistoryVariableType.Pressure:
                    series = Tank.GetPressureData();
                    break;
                case HistoryVariableType.CummulativeOilProduction:
                    series = Tank.GetCummulativeOilProducedData();
                    break;
                case HistoryVariableType.CummulativeGasProduction:
                    series = Tank.GetCummulativeGasProducedData();
                    break;
                case HistoryVariableType.CummulativeWaterInjection:
                    series = Tank.GetCummulativeWaterInjectedData();
                    break;
                case HistoryVariableType.CummulativeGasInjection:
                    series = Tank.GetCummulativeGasInjectedData();
                    break;
                case HistoryVariableType.CummulativeWaterProduction:
                    series = Tank.GetCummulativeWaterProducedData();
                    break;
            }

            if (series.Count == 0) return;

            PlotChart(modelType.VariableName, series);
            modelType.IsEnabled = false;
            var sum = HistoryVariableTypes.Where(x => !x.IsEnabled).Count();
            if (sum >= 2)
            {
                HistoryVariableTypes.ToList().ForEach(x => { x.IsEnabled = false; });
            }
        }

        //public List<double> GetSimulatedData()
        //{
        //    List<double> data = null;
        //    switch (SelectedPlotItem)
        //    {
        //        case "Pressure":
        //            data = ProductionData.Select(x => x.Pressure).ToList();
        //            break;
        //        case "Cummulative Oil":
        //            data = ProductionData.Select(x => x.OilProduced).ToList();
        //            break;
        //        case "Cummulative Gas":
        //            data = ProductionData.Select(x => x.GasProduced).ToList();
        //            break;
        //        case "Cummulative Water":
        //            data = ProductionData.Select(x => x.WaterProduced).ToList();
        //            break;
        //        case "Cummulative Gas Inj":
        //            data = ProductionData.Select(x => x.GasInjected).ToList();
        //            break;
        //        case "Cummulative Water Inj":
        //            data = ProductionData.Select(x => x.WaterInjected).ToList();
        //            break;
        //    }
        //    return data;
        //}

        //public ObservableCollection<ProductionDataRow> ProductionData => new(Tank.ProductionData);
        public ObservableCollection<ProductionDataRow> ProductionData
        {
            get => productionData;
            set
            {
                productionData = value;
                RaisePropertyChanged();
            }
        }
        public Tank Tank { get; }
        public bool IsGasTank  => Tank.FlowingFluid == FluidType.Gas;
        public PasteCommand PasteCommand { get; }
        public ClearCommand ClearCommand { get; }
        public ImportProductionDataCommand ImportProductionDataCommand { get; }
        public DelegateCommand ShowTableCommand { get; }
        public DelegateCommand ShowPlotCommand { get; }
        public DelegateCommand RefreshPlotCommand { get; }
        public DelegateCommand<object> PlotVariableCommand { get; set; }
        internal Action ProductionDataChanged { get; }
        public LineChartDataSeries PrimaryAxis { get; set; }
        public LineChartDataSeries SecondaryAxis { get; set; }
        public ICartesianChartControl Chart { get; set; }
        public IChartWrapper ChartControl { get; set; }

        private bool showTable;
        public bool ShowTable
        {
            get { return showTable; }
            set
            {
                showTable = value;
                RaisePropertyChanged();
                ShowPlotCommand.RaiseCanExecuteChanged();
                ShowTableCommand.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<HistoryVariableModelType> HistoryVariableTypes { get; set; }
        private ObservableCollection<ProductionDataRow> productionData;
    }
}
