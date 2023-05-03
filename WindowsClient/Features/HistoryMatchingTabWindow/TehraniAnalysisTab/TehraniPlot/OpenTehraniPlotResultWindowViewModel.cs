using ChartUIControls.Controls;
using ChartUIControls.Controls.ChartData;
using ChartUIControls.Controls.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsClient.Shared.UIModels;
using WindowsClient.Shared.UIModels.MaterialBalance;
using WindowsClient.Shared.Visualisation;

namespace WindowsClient.Features.HistoryMatchingTabWindow.TehraniAnalysisTab.TehraniPlot
{
    internal class OpenTehraniPlotResultWindowViewModel
    {
        public OpenTehraniPlotResultWindowViewModel(TehraniAnalysisTabViewModel viewModel,
            Func<IChartWrapper> getChartControl, RegressionResult regressionResult)
        {
            AnalysisViewModel = viewModel;
            ChartControl = getChartControl();
            Result = regressionResult;

            PlotMeasuredNp();
            ChartControl.RefreshPlot();
            PlotResult(regressionResult);
            ChartControl.ReplotChart();
            SetCurrentTankResult(regressionResult);
        }

        private void PlotResult(RegressionResult newResult)
        {
            if (newResult == null) return;
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

            PlotSelectedPoints1(newResult, ChartControl);
        }

        private void PlotMeasuredNp()
        {
            ChartControl.Title = $"Tehrani Analysis Plot for {AnalysisViewModel.Tank.Name}";
            ChartControl.GetXAxis().Title = AnalysisViewModel.Tank.FlowingFluid == FluidType.Oil ? "Cumulative Oil Production" : "Cumulative Gas Production";
            ChartControl.GetLeftYAxis(1).Title = "Pressure"; //TODO: Axis to include units
            ChartControl.GetXAxis().ExtendAxisRange = false;
            var historyPlot = AnalysisViewModel.Tank.GetPressureVsCummulativeProduction();            
            historyPlot.Color = "Blue";            
            historyPlot.Name = AnalysisViewModel.Tank.FlowingFluid == FluidType.Oil ? "Measured Np" : "Measured Gp";
            //ChartControl.GetXAxis().AutomaticTickGeneration = false;
            //ChartControl.GetXAxis().Xmin = 0;
            //ChartControl.GetXAxis().XTick = 5;
            //ChartControl.GetXAxis().Xmax = historyPlot.Select(x => x.X).Max() + 10;
            
            LineChartDataSeries lineDataSeries = new LineChartDataSeries();
            lineDataSeries.LineSeries.Tag = lineDataSeries;
            lineDataSeries.SeriesName = AnalysisViewModel.Tank.FlowingFluid == FluidType.Oil ? "Measured Np" : "Measured Gp";
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

            ChartControl.Data.Add(lineDataSeries);
            ChartControl.itemSelector.AddItem(ChartControl as ChartWrapper);
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

        private void SetCurrentTankResult(RegressionResult result)
        {
            Tank = Ioc.Resolve<AutoMapper.IMapper>().Map<Tank>(AnalysisViewModel.Tank);
            Tank.STOIP.CurrentValue.DisplayValue = result.HistoryMatchingVariables.STOIP.BestFitValue;
            Tank.Thickness.CurrentValue.DisplayValue = result.HistoryMatchingVariables.Thickness.BestFitValue;
            Tank.Radius.CurrentValue.DisplayValue = result.HistoryMatchingVariables.Radius.BestFitValue;
            Tank.Length.CurrentValue.DisplayValue = result.HistoryMatchingVariables.Length.BestFitValue;
            Tank.Width.CurrentValue.DisplayValue = result.HistoryMatchingVariables.Width.BestFitValue;
            Tank.GasCap.CurrentValue.DisplayValue = result.HistoryMatchingVariables.GasCap.BestFitValue;
            Tank.Aquifer.EncroachmentAngle.CurrentValue.DisplayValue = result.HistoryMatchingVariables.EncroachmentAngle.BestFitValue;
            Tank.Aquifer.OuterInnerRadiusRatio.CurrentValue.DisplayValue = result.HistoryMatchingVariables.OuterInnerRadius.BestFitValue;
            Tank.Aquifer.Volume.CurrentValue.DisplayValue = result.HistoryMatchingVariables.Volume.BestFitValue;
            Tank.Rock.Permeability.CurrentValue.DisplayValue = result.HistoryMatchingVariables.Permeability.BestFitValue;
            Tank.Rock.Anisotropy.CurrentValue.DisplayValue = result.HistoryMatchingVariables.Anisotropy.BestFitValue;
            Tank.Rock.Porosity.CurrentValue.DisplayValue = result.HistoryMatchingVariables.Porosity.BestFitValue;
            Tank.GIIP.CurrentValue.DisplayValue = result.HistoryMatchingVariables.GIIP.BestFitValue;
        }

        public TehraniAnalysisTabViewModel AnalysisViewModel { get; }
        public RegressionResult Result { get; }
        public Tank Tank { get; set; }
        public IChartWrapper ChartControl { get; set; }
    }
}
