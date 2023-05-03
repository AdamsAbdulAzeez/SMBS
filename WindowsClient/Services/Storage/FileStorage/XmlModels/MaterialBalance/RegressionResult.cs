using ChartUIControls.Controls;
using System;
using System.Collections.Generic;
using WindowsClient.Shared.UIModels;
using WindowsClient.Shared.UIModels.MaterialBalance;

namespace WindowsClient.Services.Storage.FileStorage.XmlModels.MaterialBalance
{
    public class RegressionResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<int> SelectedData { get; set; } = new();
        public XYDataSeries TurnOffPoints { get; set; }
        public List<DataPointModel2> SelectedDataPoint { get; set; } = new();
        public XYDataSeries EstimatedNpSeries { get; set; }
        public XYDataSeries SelectedTdSeries { get; set; }
        public XYDataSeries TdSeriesForReservoirRadiusSeries { get; set; }
        public List<XYDataSeries> WdSeriesAtOtherRadi { get; set; }
        public List<NewWdSeriesAtOtherRadi> NewWdSeriesAtOtherRadi { get; set; }
        public HistoryMatchingVariables HistoryMatchingVariables { get; set; }
        public List<ProductionEstimationResultRow> ProductionEstimationResult { get; set; }
    }
}
