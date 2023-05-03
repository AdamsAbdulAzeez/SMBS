namespace WindowsClient.Shared.UIModels.MaterialBalance
{
    public class PvtDataRow
    {
        public double Temperature { get; set; }
        public double Pressure { get; set; }
        public double BubblePoint { get; set; }
        public double GasOilRatio { get; set; }
        public double OilFVF { get; set; }
        public double OilViscosity { get; set; }
        public double ZFactor { get; set; }
        public double GasFVF { get; set; }
        public double GasViscosity { get; set; }
        public double OilDensity { get; set; }
        public double GasDensity { get; set; }
        public double WaterFVF { get; set; }
        public double WaterViscosity { get; set; }
        public double WaterDensity { get; set; }
        public double WaterCompressibility { get; set; }
        public double DewPoint { get; set; }
        public double PseudoPressure { get; set; }
        public double ReservoirCGR { get; set; }
        public double VaporizedCGR { get; set; }
        public double VapourisedWGR { get; set; }
    }
}
