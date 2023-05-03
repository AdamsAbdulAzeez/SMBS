namespace WindowsClient.Shared.UIModels.MaterialBalance
{
    public class SeparatorConfiguration
    {
        public SeparatorStage SeparatorStage { get; set; }
        public double Tsep1 { get; set; } = 60;
        public double Tsep2 { get; set; }
        public double Tsep3 { get; set; }
        public double Psep1 { get; set; } = 14.7;
        public double Psep2 { get; set; }
        public double Psep3 { get; set; }
        public double GORsep1 { get; set; }
        public double GORsep2 { get; set; }
        public double GORsep3 { get; set; }
    }
}
