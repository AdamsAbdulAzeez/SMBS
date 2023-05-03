using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Input;
using ScottPlot;
using WindowsClient.Shared.UIModels;

namespace WindowsClient.Shared.Visualisation.CartesianChart
{
    /// <summary>
    /// Interaction logic for CartesianChartControl.xaml
    /// </summary>
    public partial class CartesianChartControl : ICartesianChartControl
    {
        public Plot Plot { get; set; }
        internal CartesianChartControl()
        {
            InitializeComponent();
            Plot = PlotArea.Plot;
            PlotArea.Configuration.LeftClickDragPan = false;
            PlotArea.Configuration.ScrollWheelZoom = false;
            SetTitle("Chart Title");
            SetPrimaryYAxisTitle("Y Axis");
            SetPrimaryXAxisTitle("X Axis");
            Plot.Style(Color.FromName("Transparent"), Color.FromName("Transparent"), Color.FromName("#ddd"));
            Plot.YAxis.MajorGrid(true, Color.FromArgb(40, Color.Black));
            Plot.XAxis.MajorGrid(true, Color.FromArgb(40, Color.Black));
            Plot.YAxis.MinorGrid(true, Color.FromArgb(20, Color.Black));
            Plot.XAxis.MinorGrid(true, Color.FromArgb(20, Color.Black));
            PlotArea.MouseMove += PlotArea_MouseMove;
        }

        private void PlotArea_MouseMove(object sender, MouseEventArgs e)
        {
            var mousePos = e.MouseDevice.GetPosition(PlotArea);
            double mouseX = Plot.GetCoordinateX((float)mousePos.X);
            double mouseY = Plot.GetCoordinateY((float)mousePos.Y);

            PositionTextChanged?.Invoke((mouseX, mouseY));
        }

        public void SetTitle(string title) => Plot.Title(title);

        public void SetPrimaryXAxisTitle(string primaryAxisTitle) => Plot.XAxis.Label(primaryAxisTitle);
        public void SetSecondaryXAxisTitle(string secondaryXAxisTitle) => Plot.XAxis2.Label(secondaryXAxisTitle);
        public void SetPrimaryYAxisTitle(string primaryYAxisTitle) => Plot.YAxis.Label(primaryYAxisTitle);
        public void SetSecondaryYAxisTitle(string secondaryYAxisTitle) => Plot.YAxis2.Label(secondaryYAxisTitle);

        void ICartesianChartControl.AddSeries(IEnumerable<XYDataSeries> seriesCollection)
        {
            foreach (var series in seriesCollection)
            {
                AddScatter(series, series.Color, series.Name, series.ShowLine, series.LineWidth);
            }
            PlotArea.Refresh();
        }

        void ICartesianChartControl.AddSeries(IEnumerable<TimeSeries> seriesCollection)
        {
            foreach (var series in seriesCollection)
            {
                AddScatter(series, series.Color, series.Name, series.ShowLine, series.LineWidth);
            }
            PlotArea.Refresh();
        }

        void ICartesianChartControl.SetLogarithmicXAxis()
        {
            Plot.XAxis.TickLabelFormat((double y) => Math.Pow(10, y).ToString("N0"));

            Plot.XAxis.MinorLogScale(true);
            Plot.YAxis.MajorGrid(true, Color.FromArgb(80, Color.Black));
            Plot.XAxis.MinorGrid(true, Color.FromArgb(20, Color.Black));
            Plot.XAxis.MajorGrid(true, Color.FromArgb(80, Color.Black));
        }

        void ICartesianChartControl.SetPrimaryXRange(Range range)
        {
            if (range.Minimum.HasValue)
            {
                // remember the old axis limits
                Plot.SetAxisLimitsX(range.Minimum.Value, Plot.GetAxisLimits(0, 0).XMax);
            }

            if (range.Maximum.HasValue)
            {

                Plot.SetAxisLimitsX(Plot.GetAxisLimits(0, 0).XMin, range.Maximum.Value);
            }
        }

        void ICartesianChartControl.SetPrimaryYRange(Range range)
        {
            if (range.Minimum.HasValue)
            {
                // remember the old axis limits
                Plot.SetAxisLimitsY(range.Minimum.Value, Plot.GetAxisLimits(0, 0).XMax);
            }

            if (range.Maximum.HasValue)
            {

                Plot.SetAxisLimitsX(Plot.GetAxisLimits(0, 0).YMin, range.Maximum.Value);
            }
        }

        public void YLabelCallBack(Func<double, string> logTickLabels) => Plot.YAxis.TickLabelFormat(logTickLabels);

        void AddScatter(XYDataSeries series, string color, string label, bool showLine, float lineWidth, bool useSpline = false)
        {
            if (series.Count == 0) return;

            double[] xs = series.Select(x => x.X).ToArray();
            double[] ys = series.Select(x => x.Y).ToArray();

            if (XYDataSeriesCollection.Exists(d => d.Name == series.Name && d.YAxisLabel == series.YAxisLabel)) return;
           (xs, ys) = useSpline ?  ScottPlot.Statistics.Interpolation.Chaikin.InterpolateXY(xs, ys, 10) : (xs, ys);

            var seriesColor = Plot.GetNextColor();

            if (!string.IsNullOrEmpty(color)) seriesColor = System.Drawing.Color.FromName(color);

            var scatter = Plot.AddScatter(
                xs,
                ys,
                markerSize: series.MarkerSize,
                color: seriesColor,
                lineWidth: showLine ? lineWidth : 0,
                markerShape: !series.ShowMarker ? MarkerShape.none : MarkerShape.filledCircle,
                label: label);


            if (XYDataSeriesCollection.Count == 0)
            {
                SetPrimaryYAxisTitle(series.YAxisLabel);
                XYDataSeriesCollection.Add(series);
                return;
            }

            if (!XYDataSeriesCollection.Exists(d => d.YAxisLabel == series.YAxisLabel))
            {
                var axisPosition = (XYDataSeriesCollection.Count + 1) % 2 == 0 ? 
                    ScottPlot.Renderable.Edge.Right : ScottPlot.Renderable.Edge.Left;
                var newAxis = Plot.AddAxis(axisPosition, axisIndex: XYDataSeriesCollection.Count == 0 ? 2 : (XYDataSeriesCollection.Count + 1));
                newAxis.Label(series.YAxisLabel);
                axes.Add(newAxis);
                scatter.YAxisIndex = XYDataSeriesCollection.Count + 1;
                newAxis.AxisIndex = scatter.YAxisIndex;
                Plot.Legend(true, Alignment.UpperLeft);
            }
            XYDataSeriesCollection.Add(series);
            Plot.Legend(true, Alignment.UpperLeft);
        }

        void AddScatter(TimeSeries series, string color, string label, bool showLine, float lineWidth)
        {
            if (series.Count == 0) return;

            double[] xs = series.Select(x => x.Date.ToOADate()).ToArray();
            double[] ys = series.Select(x => x.Value).ToArray();

            var seriesColor = Plot.GetNextColor();

            if (!string.IsNullOrEmpty(color)) seriesColor = System.Drawing.Color.FromName(color);

            var scatter = Plot.AddScatter(
                xs,
                ys,
                markerSize: 5F,
                color: seriesColor,
                lineWidth: showLine ? lineWidth : 0,
                markerShape: !series.ShowMarker ? MarkerShape.none : MarkerShape.filledCircle,
                label: label);
            Plot.XAxis.DateTimeFormat(true);


            Plot.Legend(true, Alignment.UpperRight);
        }

        void ICartesianChartControl.Clear()
        {
            XYDataSeriesCollection = new();
            axes.ForEach(axis => Plot.RemoveAxis(axis));
            axes = new();
            SetPrimaryYAxisTitle("Y Axis");
            SetPrimaryXAxisTitle("X Axis");
            Plot.Clear();
            PlotArea.Refresh();
        }

        public void Refresh() => PlotArea.Refresh();

        private List<XYDataSeries> XYDataSeriesCollection = new();
        private List<ScottPlot.Renderable.Axis> axes = new();

        public event Action<(double, double)> PositionTextChanged;
    }
}
