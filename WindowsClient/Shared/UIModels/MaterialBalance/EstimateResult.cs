using System.Collections.Generic;

namespace WindowsClient.Shared.UIModels.MaterialBalance
{
    public class EstimateResult
    {
        public string Name { get; set; }
        public XYDataSeries EstimatedNpSeries { get; set; }
        public XYDataSeries SelectedTdSeries { get; set; }
        public XYDataSeries TdSeriesForReservoirRadiusSeries { get; set; }
        public List<XYDataSeries> WdSeriesAtOtherRadi { get; set; }
        public List<NewWdSeriesAtOtherRadi> NewWdSeriesAtOtherRadi { get; set; }
        public List<ProductionEstimationResultRow> ProductionEstimationResult { get; set; }
    }
}
