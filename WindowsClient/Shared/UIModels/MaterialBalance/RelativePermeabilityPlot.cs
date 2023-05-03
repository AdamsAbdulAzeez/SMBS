using System.Collections.Generic;

namespace WindowsClient.Shared.UIModels.MaterialBalance
{
    public class RelativePermeabilityPlot
    {
        public List<double> So { get; set; } = new List<double>();
        public List<double> Sw { get; set; } = new List<double>();
        public List<double> Sg { get; set; } = new List<double>();
        public List<double> Kro { get; set; } = new List<double>();
        public List<double> Krw { get; set; } = new List<double>();
        public List<double> Krg { get; set; } = new List<double>();
    }
}
