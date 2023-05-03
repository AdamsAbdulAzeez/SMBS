using ChartUIControls.Controls;
using ChartUIControls.Controls.ChartData;
using Prism.Mvvm;
using System;
using System.Collections.Generic;

namespace WindowsClient.Shared.UIModels.MaterialBalance
{
    public class RegressionResult : BindableBase
    {
        private HistoryMatchingVariables historyMatchingVariables;

        public Guid Id { get; } = Guid.NewGuid();
        public string Name { get; set; }
        public List<int> SelectedData { get; set; } = new List<int>();
        public XYDataSeries TurnOffPoints { get; set; }
        public ScatterChartDataSeries SelectedPoints { get; set; }
        public List<DataPointModel2> SelectedDataPoint { get; set; } = new List<DataPointModel2>();
        public XYDataSeries EstimatedNpSeries { get; set; }
        public XYDataSeries SelectedTdSeries { get; set; }
        public XYDataSeries TdSeriesForReservoirRadiusSeries { get; set; }
        public List<XYDataSeries> WdSeriesAtOtherRadi { get; set; }
        public List<NewWdSeriesAtOtherRadi> NewWdSeriesAtOtherRadi { get; set; }
        public HistoryMatchingVariables HistoryMatchingVariables
        {
            get => historyMatchingVariables;
            set { historyMatchingVariables = value; RaisePropertyChanged(); }
        }
        public List<ProductionEstimationResultRow> ProductionEstimationResult { get; set; }
    }

    public class NewWdSeriesAtOtherRadi
    {
        public string Name { get; set; }
        public XYDataSeries WdAtOtherRadi { get; set; } = new XYDataSeries();
    }
}
