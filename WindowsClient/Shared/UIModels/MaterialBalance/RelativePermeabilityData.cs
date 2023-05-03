namespace WindowsClient.Shared.UIModels.MaterialBalance
{
    public class RelativePermeabilityData
    {
        public RelativePermeabilityData()
        {
            OilRelPerm = new() { Name="Kro"};
            WaterRelPerm = new() { Name="Krw"};
            GasRelPerm = new() { Name = "Krg" };
        }
        public RelativePermeabilityDataRow OilRelPerm { get; set; }
        public RelativePermeabilityDataRow WaterRelPerm { get; set; }
        public RelativePermeabilityDataRow GasRelPerm { get; set; }
    }
}
