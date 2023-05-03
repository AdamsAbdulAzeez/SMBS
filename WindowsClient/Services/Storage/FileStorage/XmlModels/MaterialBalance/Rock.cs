namespace WindowsClient.Services.Storage.FileStorage.XmlModels.MaterialBalance
{
    public class Rock
    {
        public BoundedVariable Permeability { get; set; }
        public BoundedVariable Porosity { get; set; }
        public BoundedVariable Anisotropy { get; set; }
        public double Compressibility { get; set; }
    }
}
