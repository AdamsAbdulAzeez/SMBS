namespace WindowsClient.Services.Storage.FileStorage.XmlModels.MaterialBalance
{
    public class RegressionVariable 
    {
        public string Name { get; set; }
        public bool ToBeOptimized { get; set; }
        public double BestFitValue { get; set; }
        public double LowerBound { get; set; }
        public double Start { get; set; }
        public double UpperBound { get; set; }
    }
}
