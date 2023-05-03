using System;

namespace WindowsClient.Shared.UIModels.MaterialBalance
{
    public class FractionalMatchResult
    {
        public double[] Muo = Array.Empty<double>(), Muw = Array.Empty<double>(), Mug = Array.Empty<double>();
        public FractionalMatchResultRow OilMatch { get; set; } = new();
        public FractionalMatchResultRow GasMatch { get; set; } = new();
        public FractionalMatchResultRow WaterMatch { get; set; } = new();
    }
}
