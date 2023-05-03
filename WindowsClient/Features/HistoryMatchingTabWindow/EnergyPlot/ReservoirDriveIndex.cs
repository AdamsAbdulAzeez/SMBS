using System.Collections.Generic;

namespace WindowsClient.Features.HistoryMatchingTabWindow.WdPlot
{
    public class ReservoirDriveIndex
    {
        public string SeriesName { get; set; }
        public List<double> IndexValues { get; set; } = new List<double>();
        public System.Windows.Media.Brush color { get; set; }
    }
}
