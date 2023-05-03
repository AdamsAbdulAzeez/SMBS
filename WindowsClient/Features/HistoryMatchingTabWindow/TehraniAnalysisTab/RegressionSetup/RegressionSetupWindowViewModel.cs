using System;
using AutoMapper;
using ChartUIControls.Controls;
using ChartUIControls.Controls.ChartData;
using Prism.Mvvm;
using WindowsClient.Features.HistoryMatchingTabWindow.TehraniAnalysisTab.RegressionSetup.Commands;
using WindowsClient.Services.Calculation;
using WindowsClient.Shared.UIModels.MaterialBalance;
using WindowsClient.Shared.Visualisation.CartesianChart;
using WindowsClient.Shared.Visualisation;
using System.Linq;
using ChartUIControls.Controls.Utils;
using System.Threading.Tasks;
using CypherCrescent.Units.Variables;

namespace WindowsClient.Features.HistoryMatchingTabWindow.TehraniAnalysisTab.RegressionSetup
{
    internal class RegressionSetupWindowViewModel : BindableBase
    {
        public RegressionSetupWindowViewModel(
            TehraniAnalysisTabViewModel viewModel,
            IMapper mapper,
            Func<IChartWrapper> getChartControl,
            ICalculationServices calculator)
        {
            AnalysisViewModel = viewModel;
            _mapper = mapper;

            ChartControl = getChartControl();
            LoadChart(calculator).Await();

            HistoryMatchingVariables = new HistoryMatchingVariables(_mapper.Map<Tank>(AnalysisViewModel.Tank));
            RunRegressionCommand = new RunRegressionCommand(calculator, this);
            UpdateRegressionCommand = new UpdateRegressionCommand(calculator, this);
            AcceptRegressionResultCommand = new AcceptRegressionResultCommand(this);
            RunRegressionCommand.RegressionStarted += OnRegressionStarted;
            RunRegressionCommand.RunCompleted += OnRegressionComplete;
            UpdateRegressionCommand.UpdateCompleted += OnUpdateComplete;
            SwapStartAndBestFitCommand = new SwapStartAndBestFitCommand(this);
        }

        private async Task LoadChart(ICalculationServices calculator)
        {
            ChartControl.RefreshPlot();
            if (AnalysisViewModel.Chart.SelectedRegressionResult != null && AnalysisViewModel.Chart.SelectedRegressionResult.TurnOffPoints != null && AnalysisViewModel.Chart.SelectedRegressionResult.TurnOffPoints.Count > 0)
            {
                ChartControl.SelectedRegressionResult.SelectedData = AnalysisViewModel.Chart.SelectedRegressionResult.SelectedData;
                ChartControl.SelectedRegressionResult.TurnOffPoints = AnalysisViewModel.Chart.SelectedRegressionResult.TurnOffPoints;
                ChartControl.SelectedRegressionResult.SelectedDataPoint = AnalysisViewModel.Chart.SelectedRegressionResult.SelectedDataPoint;
                PlotMeasuredNp();
                var enP = await calculator.HistoryMatchingService.SingleTankEstimateAsync(Tank, ChartControl, ChartControl.SelectedRegressionResult.TurnOffPoints);
                ChartControl.EstimatedNpPlot(enP, Tank);
                PlotRegressionSelectedPoints(ChartControl);
            }
            else
            {
                PlotMeasuredNp();
                var nP = await calculator.HistoryMatchingService.SingleTankEstimateAsync(Tank, ChartControl, null);
                ChartControl.EstimatedNpPlot(nP,Tank);
                PlotSelectedPoints(ChartControl);
            }
            ChartControl.ReplotChart();
        }

        public void PlotSelectedPoints(IChartWrapper chart)
        {
            chart.GetXAxis().AxisType = DataTypeEnum.Linear;
            chart.SelectedRegressionResult.SelectedPoints = new ScatterChartDataSeries();
            chart.SelectedRegressionResult.SelectedPoints.YAxis = chart.GetLeftYAxis(1).ID.ToString();
            chart.SelectedRegressionResult.SelectedPoints.YaxisDataType = DataTypeEnum.Logarithmic;
            chart.SelectedRegressionResult.SelectedPoints.SymbolSize = 5;
            chart.SelectedRegressionResult.SelectedPoints.FillPattern = FillPatternEnum.Solid;
            chart.SelectedRegressionResult.SelectedPoints.SymbolColor = ConverterUtils.StringToBrush("LightGray");
            chart.SelectedRegressionResult.SelectedPoints.ScatterSeries.IsContextMenuVisible = false;
            chart.SelectedRegressionResult.SelectedPoints.IsXAxisExtended = false;
            chart.SelectedRegressionResult.SelectedPoints.SeriesName = "Selected Point";

            chart.SelectedRegressionResult.SelectedPoints.GetLegendItem().Visibility = ConverterUtils.GetVisibility("Collapsed");
        }

        public void PlotRegressionSelectedPoints(IChartWrapper chart)
        {
            if (ChartControl.TurnedOffPoints.Count == 0)
            {
                foreach (var item in chart.SelectedRegressionResult.SelectedDataPoint)
                    ChartControl.TurnedOffPoints.Add(((double)item.XValue, (double)item.YValue));
            }

            ChartControl.GetXAxis().AxisType = DataTypeEnum.Linear;
            ChartControl.SelectedRegressionResult.SelectedPoints = new ScatterChartDataSeries();
            ChartControl.SelectedRegressionResult.SelectedPoints.YAxis = ChartControl.GetLeftYAxis(1).ID.ToString();
            ChartControl.SelectedRegressionResult.SelectedPoints.YaxisDataType = DataTypeEnum.Logarithmic;
            ChartControl.SelectedRegressionResult.SelectedPoints.SymbolSize = 10;
            ChartControl.SelectedRegressionResult.SelectedPoints.FillPattern = FillPatternEnum.Solid;
            ChartControl.SelectedRegressionResult.SelectedPoints.SymbolColor = ConverterUtils.StringToBrush("LightGray");
            ChartControl.SelectedRegressionResult.SelectedPoints.IsXAxisExtended = false;
            ChartControl.SelectedRegressionResult.SelectedPoints.SeriesName = "Selected Point";

            ChartControl.SelectedRegressionResult.SelectedPoints.GetLegendItem().Visibility = ConverterUtils.GetVisibility("Collapsed");

            //----------------------------//
            ChartControl.SelectedRegressionResult.SelectedDataPoint.Clear();
            //----------------------------//

            for (int i = 0; i < ChartControl.TurnedOffPoints.Count; i++)
            {
                DataPoint thisDataPoint = new DataPoint();
                thisDataPoint.XValue = ChartControl.TurnedOffPoints[i].X;
                thisDataPoint.YValue = ChartControl.TurnedOffPoints[i].Y;
                thisDataPoint.Tooltip = $"X: {ChartControl.TurnedOffPoints[i].X}   Y: {ChartControl.TurnedOffPoints[i].Y}";
                ChartControl.SelectedRegressionResult.SelectedPoints.Data.Add(thisDataPoint);

                //Added this
                //----------------------------//
                DataPointModel2 modelPoint = new DataPointModel2();
                modelPoint.XValue = ChartControl.TurnedOffPoints[i].X;
                modelPoint.YValue = ChartControl.TurnedOffPoints[i].Y;
                modelPoint.ToolTip = $"X: {ChartControl.TurnedOffPoints[i].X}   Y: {ChartControl.TurnedOffPoints[i].Y}";
                ChartControl.SelectedRegressionResult.SelectedDataPoint.Add(modelPoint);
                //----------------------------//
            }

            ChartControl.Data.Add(ChartControl.SelectedRegressionResult.SelectedPoints);

            ChartControl.itemSelector.AddItem(ChartControl as ChartWrapper);
        }

        private void OnRegressionStarted()
        {
            Result = null;
            IsRunningRegression = true;
            RaiseCommandsCanExecuteChanged();
        }

        private void OnRegressionComplete(RegressionResult newResult)
        {
            Result = newResult;
            IsRunningRegression = false;
            RaiseCommandsCanExecuteChanged();

            //TODO: If null show error;
            if (newResult == null) return;

            HistoryMatchingVariables.SetBestFits(newResult.HistoryMatchingVariables);
            RaisePropertyChanged(nameof(HistoryMatchingVariables));
            ChartControl.RefreshPlot();
            PlotMeasuredNp();
            newResult.EstimatedNpSeries.Color = "firebrick";
            newResult.EstimatedNpSeries.Name = AnalysisViewModel.Tank.FlowingFluid == FluidType.Oil ? "Estimated Np" : "Estimated Gp";
            newResult.EstimatedNpSeries.ShowLine = true;
            newResult.EstimatedNpSeries.LineWidth = 3;

            LineChartDataSeries lineDataSeries = new LineChartDataSeries();
            lineDataSeries.LineSeries.Tag = lineDataSeries;
            lineDataSeries.SeriesName = AnalysisViewModel.Tank.FlowingFluid == FluidType.Oil ? "Estimated Np" : "Estimated Gp";
            lineDataSeries.SymbolColor = ConverterUtils.StringToBrush("firebrick");
            lineDataSeries.LineColor = ConverterUtils.StringToBrush("firebrick");
            lineDataSeries.SymbolSize = 0;
            lineDataSeries.LineThickness = 2;

            foreach (var item in newResult.EstimatedNpSeries)
            {
                lineDataSeries.Data.Add(new DataPoint { XValue = item.X, YValue = item.Y, Tooltip = $"X: {item.X}   Y: {item.Y}" });
            }

            ChartControl.Data.Add(lineDataSeries);
            ChartControl.itemSelector.AddItem(ChartControl as ChartWrapper);
            //ChartControl.AddSeries(new [] { newResult.EstimatedNpSeries });

            PlotRegressionSelectedPoints(ChartControl);
            ChartControl.ReplotChart();
        }

        public void SetCurrentTankResult()
        {
            Tank.STOIP.CurrentValue = HistoryMatchingVariables.STOIP.BestFitValue;
            Tank.Thickness.CurrentValue = HistoryMatchingVariables.Thickness.BestFitValue;
            Tank.Radius.CurrentValue = HistoryMatchingVariables.Radius.BestFitValue;
            Tank.Length.CurrentValue = HistoryMatchingVariables.Length.BestFitValue;
            Tank.Width.CurrentValue = HistoryMatchingVariables.Width.BestFitValue;
            Tank.GasCap.CurrentValue = HistoryMatchingVariables.GasCap.BestFitValue;
            Tank.Aquifer.EncroachmentAngle.CurrentValue = HistoryMatchingVariables.EncroachmentAngle.BestFitValue;
            Tank.Aquifer.OuterInnerRadiusRatio.CurrentValue = HistoryMatchingVariables.OuterInnerRadius.BestFitValue;
            Tank.Aquifer.Volume.CurrentValue = HistoryMatchingVariables.Volume.BestFitValue;
            Tank.Rock.Permeability.CurrentValue = HistoryMatchingVariables.Permeability.BestFitValue;
            Tank.Rock.Anisotropy.CurrentValue = HistoryMatchingVariables.Anisotropy.BestFitValue;
            Tank.Rock.Porosity.CurrentValue = HistoryMatchingVariables.Porosity.BestFitValue;
            Tank.GIIP.CurrentValue = HistoryMatchingVariables.GIIP.BestFitValue;
        }

        public void UpdateTankVariables(Tank tank, HistoryMatchingVariables variables)
        {
            tank.Thickness.CurrentValue.DisplayValue = variables.Thickness.BestFitValue;
            tank.STOIP.CurrentValue.DisplayValue = variables.STOIP.BestFitValue;
            tank.GasCap.CurrentValue.DisplayValue = variables.GasCap.BestFitValue;
            tank.GIIP.CurrentValue.DisplayValue = variables.GIIP.BestFitValue;
            tank.Length.CurrentValue.DisplayValue = variables.Length.BestFitValue;
            tank.Radius.CurrentValue.DisplayValue = variables.Radius.BestFitValue;
            tank.Rock.Anisotropy.CurrentValue.DisplayValue = variables.Anisotropy.BestFitValue;
            tank.Rock.Permeability.CurrentValue.DisplayValue = variables.Permeability.BestFitValue;
            tank.Rock.Porosity.CurrentValue.DisplayValue = variables.Porosity.BestFitValue;
            tank.Aquifer.OuterInnerRadiusRatio.CurrentValue.DisplayValue = variables.OuterInnerRadius.BestFitValue;
            tank.Aquifer.EncroachmentAngle.CurrentValue.DisplayValue = variables.EncroachmentAngle.BestFitValue;
            tank.Aquifer.Volume.CurrentValue.DisplayValue = variables.Volume.BestFitValue;
            tank.Width.CurrentValue.DisplayValue = variables.Width.BestFitValue;
        }

        private void OnUpdateComplete(EstimateResult result)
        {
            if (result == null) return;

            Result.HistoryMatchingVariables.SetBestFits(HistoryMatchingVariables);
            Result.EstimatedNpSeries = result.EstimatedNpSeries;
            ChartControl.RefreshPlot();
            PlotMeasuredNp();
            result.EstimatedNpSeries.Color = "firebrick";
            result.EstimatedNpSeries.Name = Tank.FlowingFluid == FluidType.Oil ? "Estimated Np" : "Estimated Gp";
            result.EstimatedNpSeries.ShowLine = true;
            result.EstimatedNpSeries.LineWidth = 3;

            LineChartDataSeries lineDataSeries = new LineChartDataSeries();
            lineDataSeries.LineSeries.Tag = lineDataSeries;
            lineDataSeries.SeriesName = Tank.FlowingFluid == FluidType.Oil ? "Estimated Np" : "Estimated Gp";
            lineDataSeries.SymbolColor = ConverterUtils.StringToBrush("firebrick");
            lineDataSeries.LineColor = ConverterUtils.StringToBrush("firebrick");
            lineDataSeries.SymbolSize = 0;
            lineDataSeries.LineThickness = 2;

            foreach (var item in result.EstimatedNpSeries)
            {
                lineDataSeries.Data.Add(new DataPoint { XValue = item.X, YValue = item.Y, Tooltip = $"X: {item.X}   Y: {item.Y}" });
            }

            ChartControl.Data.Add(lineDataSeries);
            ChartControl.itemSelector.AddItem(ChartControl as ChartWrapper);

            PlotRegressionSelectedPoints(ChartControl);
            ChartControl.ReplotChart();
        }

        public void RaiseCommandsCanExecuteChanged()
        {
            RunRegressionCommand.RaiseCanExecuteChanged();
            SwapStartAndBestFitCommand.RaiseCanExecuteChanged();
            AcceptRegressionResultCommand.RaiseCanExecuteChanged();
        }

        private void PlotMeasuredNp()
        {
            ChartControl.Title = $"Tehrani Analysis Plot for {AnalysisViewModel.Tank.Name}";
            ChartControl.XAxisLabel = "Cumulative Production";
            ChartControl.YAxisLabel = "Pressure"; //TODO: Axis to include units
            ChartControl.GetLeftYAxis(1).Title = "Pressure (Psia)";
            ChartControl.GetXAxis().Title = AnalysisViewModel.Tank.FlowingFluid == FluidType.Oil ? "Cumulative Oil Production(MMSTB)" : "Cumulative Gas Production (MMSCF)";
            ChartControl.GetXAxis().ExtendAxisRange = false;
            var historyPlot = AnalysisViewModel.Tank.GetPressureVsCummulativeProduction();
            historyPlot.Color = "Blue";
            historyPlot.Name = AnalysisViewModel.Tank.FlowingFluid == FluidType.Oil ? "Measured Np" : "Measured Gp";
            //ChartControl.GetXAxis().AutomaticTickGeneration = false;
            //ChartControl.GetXAxis().Xmin = 0;
            //ChartControl.GetXAxis().Xmax = historyPlot.Select(x => x.X).Max();

            LineChartDataSeries lineDataSeries = new LineChartDataSeries();
            lineDataSeries.LineSeries.Tag = lineDataSeries;
            lineDataSeries.SeriesName = AnalysisViewModel.Tank.FlowingFluid == FluidType.Oil ? "Measured Np" : "Measured Gp";
            lineDataSeries.SymbolColor = ConverterUtils.StringToBrush("Blue");
            lineDataSeries.SymbolSize = 10;
            lineDataSeries.LineColor = ConverterUtils.StringToBrush("Blue");
            lineDataSeries.LinePattern = LinePatternEnum.Solid;
            lineDataSeries.LineThickness = 0;
            lineDataSeries.LineType = LineTypeEnum.StairStep;

            foreach (var item in historyPlot)
            {
                lineDataSeries.Data.Add(new DataPoint { XValue = item.X, YValue = item.Y, Tooltip = $"X: {item.X}   Y: {item.Y}" });
            }

            //WindowsClient.Shared.Utils.ChartHelper.Update
            ChartControl.Data.Add(lineDataSeries);
            ChartControl.itemSelector.AddItem(ChartControl as ChartWrapper);
        }

        public void AcceptResults()
        {
            SetProperties();
            ResultAccepted?.Invoke(Result);
        }

        void SetProperties()
        {
            SetVariable(Tank.STOIP, HistoryMatchingVariables.STOIP);
            SetVariable(Tank.GIIP, HistoryMatchingVariables.GIIP);
            SetVariable(Tank.Thickness, HistoryMatchingVariables.Thickness);
            SetVariable(Tank.Length, HistoryMatchingVariables.Length);
            SetVariable(Tank.Width, HistoryMatchingVariables.Width);
            SetVariable(Tank.Radius, HistoryMatchingVariables.Radius);

            SetVariable(Tank.Rock.Anisotropy, HistoryMatchingVariables.Anisotropy);
            SetVariable(Tank.Rock.Porosity, HistoryMatchingVariables.Porosity);
            SetVariable(Tank.Rock.Permeability, HistoryMatchingVariables.Permeability);

            SetVariable(Tank.Aquifer.Anisotropy, HistoryMatchingVariables.Anisotropy);
            SetVariable(Tank.Aquifer.EncroachmentAngle, HistoryMatchingVariables.EncroachmentAngle);
            SetVariable(Tank.Aquifer.OuterInnerRadiusRatio, HistoryMatchingVariables.OuterInnerRadius);
            SetVariable(Tank.Aquifer.Volume, HistoryMatchingVariables.Volume);
        }

        void SetVariable<T>(BoundedVariable<T> boundedVariable, RegressionVariable regressionVariable) where T : VariableBase, new()
        {
            boundedVariable.LowerBound.DisplayValue = regressionVariable.LowerBound;
            boundedVariable.UpperBound.DisplayValue = regressionVariable.UpperBound;
        }

        public TehraniAnalysisTabViewModel AnalysisViewModel { get; }
        public AcceptRegressionResultCommand AcceptRegressionResultCommand { get; }
        public SwapStartAndBestFitCommand SwapStartAndBestFitCommand { get; }
        public RunRegressionCommand RunRegressionCommand { get; set; }
        public UpdateRegressionCommand UpdateRegressionCommand { get; set; }
        public RegressionResult Result { get; private set; }

        public bool IsRunningRegression
        {
            get => _isRunningRegression;
            set
            {
                _isRunningRegression = value;
                RaisePropertyChanged(nameof(IsRunningRegression));
            }
        }

        public Tank Tank => AnalysisViewModel.Tank;
        private readonly IMapper _mapper;
        private bool _isRunningRegression;
        public IChartWrapper ChartControl { get; set; }
        public HistoryMatchingVariables HistoryMatchingVariables { get; set; }
        public event Action<RegressionResult> ResultAccepted;
    }
}
