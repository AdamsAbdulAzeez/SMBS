using ChartUIControls.Controls;
using ChartUIControls.Controls.ChartData;
using ChartUIControls.Controls.Events;
using ChartUIControls.Controls.Legend;
using ChartUIControls.Controls.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Media;
using WindowsClient.Shared.UIModels;
using WindowsClient.Shared.UIModels.MaterialBalance;

namespace WindowsClient.Shared.Visualisation
{
    public interface IChartWrapper
    {
        string Title { get; set; }
        bool XAxisTickMarksVisibility { get; set; }
        bool YAxisTickMarksVisibility { get; set; }
        string XAxisLabel { get; set; }
        string YAxisLabel { get; set; }
        int LeftAxisCount { get; set; }
        int RightAxisCount { get; set; }
        bool PrimaryVerticalGridLinesVisibility { get; set; }
        bool SecondaryHorizontalGridLinesVisibility { get; set; }
        bool SecondaryVerticalGridLinesVisibility { get; set; }
        bool PrimaryHorizontalGridLinesVisibility { get; set; }
        bool IsDragDropEnabled { get; set; }
        string ItemName { get; }
        bool IsSelected { get; set; }

        ChartAreaExternalY ChartArea { get; }
        ChartItemSelector itemSelector { get; set; }
        DataSeriesCollection Data { get; set; }
        Tank tank { get; set; }
        XYDataSeries TurnedOffPoints { get; set; }
        XYDataSeries TurnedOnPoints { get; set; }

        void PlotEnergyAction(List<DateTime> dateTimes, List<double> list, string seriesName, string v1, string v2, Brush color);

        Brush Fill { get; set; }
        Brush PrimaryHorizontalGridLineColor { get; set; }
        Brush PrimaryVerticalGridLineColor { get; set; }
        Brush SecondaryHorizontalGridLineColor { get; set; }
        Brush SecondaryVerticalGridLineColor { get; set; }
        RegressionResult SelectedRegressionResult { get; set; }
        ObservableCollection<RegressionResult> RegressionResults { get; set; }

        LegendPositionEnum LegendPosition { get; set; }
        LinePatternEnum PrimaryHorizontalGridLinePattern { get; set; }
        LinePatternEnum PrimaryVerticalGridLinePattern { get; set; }
        LinePatternEnum SecondaryHorizontalGridLinePattern { get; set; }
        LinePatternEnum SecondaryVerticalGridLinePattern { get; set; }

        void OnPropertyChanged([CallerMemberName] string propertyName = "");
        void RefreshPlot();
        void ReplotChart();
        void AddDataSeries(List<DataSeries> dataItems);
        void DeleteDataSeries(List<DataSeries> dataItems);
        void DeleteSeries(DataSeries series);
        void PlotMeasuredNp();
        void PlotSelectedPoints();
        void OnResultAccepted(RegressionResult result);
        void EstimatedNpPlot(EstimateResult result, Tank tank = null);
        void PlotLineAction(
            List<double> xValues, List<double> yValues,
            string seriesName, string currentXAxisTitle,
            string currentYAxisTitle, Brush color,
            int lineThickness = 2, int symbolSize = 2,
            string xMarker = "NP", string yMarker = "Pressure");
        Canvas GetPlotCanvas();
        YAxis GetRightYAxis(int position);
        XAxis GetXAxis();
        YAxis GetLeftYAxis(int position);
        YAxis GetYAxis(string id);

        ILegendPlotter GetLegendPlotter();

        event Action<RegressionResult> ResultAccepted;
        event EventHandler<IsSelectedEventArgs> IsSelectedEvent;
    }
}