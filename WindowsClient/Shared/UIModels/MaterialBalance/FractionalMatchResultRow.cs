using System;

namespace WindowsClient.Shared.UIModels.MaterialBalance
{
    public class FractionalMatchResultRow
    {
        public bool Matched { get; set; }
        public double EndPoint { get; set; }
        public double Exponent { get; set; }
        public double BreakthroughSx { get; set; }
        public double BreakthroughFx { get; set; }
        public double[] Sx { get; set; } = Array.Empty<double>();
        public double[] Fx { get; set; } = Array.Empty<double>();
        public double[] LineSx { get; set; } = Array.Empty<double>();
        public double[] LineFx { get; set; } = Array.Empty<double>();
        public double[] ProductionSx { get; set; } = Array.Empty<double>();
        public double[] ProductionFx { get; set; } = Array.Empty<double>();

        internal bool IsLineDataEmpty() => LineSx.Length == 0 || LineFx.Length == 0;
    }
}
