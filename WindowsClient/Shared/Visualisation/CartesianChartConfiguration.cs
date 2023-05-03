using System.Collections.Generic;

namespace WindowsClient.Shared.Visualisation.CartesianChart
{
    internal sealed class CartesianChartConfiguration
    {
        public bool HasSecondaryXAxis { get; set; }
        public IEnumerable<YAxisInfo> YAxes { get; set; }
        public string Title { get; set; }
        public string PrimaryXTitle { get; set; }
        public string SecondaryXTitle { get; set; }
    }
}