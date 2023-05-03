namespace WindowsClient.Shared.Visualisation.CartesianChart
{
    public struct Range
    {
        public Range(double? minimum, double? maximum)
        {
            Maximum = maximum;
            Minimum = minimum;
        }

        public double? Minimum { get; set; }
        public double? Maximum { get; set; }
    }
}