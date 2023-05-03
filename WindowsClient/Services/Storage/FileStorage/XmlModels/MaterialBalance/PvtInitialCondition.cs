namespace WindowsClient.Services.Storage.FileStorage.XmlModels.MaterialBalance
{
    public class PvtInitialCondition
    {
        public double Temperature { get; set; }
        public double GOR { get; set; }
        public double OilGravity { get; set; }
        public double GasGravity { get; set; }
        public double WaterSalinity { get; set; }
        public double MoleH2S { get; set; }
        public double MoleN2 { get; set; }
        public double MoleCO2 { get; set; }
        public double BubblePoint { get; set; }
        public double DewPoint { get; set; }
    }
}
