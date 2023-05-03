using System;

namespace WindowsClient.Services.Storage.FileStorage.XmlModels.MaterialBalance
{
    public class ProductionDataRow
    {
        public DateTime Time { get; set; }
        public double Pressure { get; set; }
        public double OilProduced { get; set; }
        public double GasProduced { get; set; }
        public double WaterProduced { get; set; }
        public double GasInjected { get; set; }
        public double WaterInjected { get; set; }
        public bool IsSelected { get; set; }
    }
}
