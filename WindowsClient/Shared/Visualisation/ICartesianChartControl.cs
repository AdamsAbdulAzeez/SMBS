using System;
using System.Collections.Generic;
using WindowsClient.Shared.UIModels;

namespace WindowsClient.Shared.Visualisation.CartesianChart
{
    /// <summary>
    /// Defines the core behaviors expected from any charting framework to be used in the application.
    /// </summary>
    public interface ICartesianChartControl
    {
        void SetTitle(string title);
        void SetPrimaryXAxisTitle(string primaryAxisTitle);
        void SetSecondaryXAxisTitle(string secondaryAxisTitle);
        void SetPrimaryYAxisTitle(string primaryYAxisTitle);
        void SetSecondaryYAxisTitle(string secondaryYAxisTitle);
        void AddSeries(IEnumerable<XYDataSeries> seriesCollection);
        void AddSeries(IEnumerable<TimeSeries> seriesCollection);
        void SetLogarithmicXAxis();
        void SetPrimaryXRange(Range range);
        void SetPrimaryYRange(Range range);
        void Refresh();
        void Clear();
        event Action<(double, double)> PositionTextChanged;

    }
}