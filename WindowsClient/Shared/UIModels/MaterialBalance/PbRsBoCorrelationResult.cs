namespace WindowsClient.Shared.UIModels.MaterialBalance
{
    public class PbRsBoCorrelationResult
    {
        public MatchingParameters PbResult { get; set; } = new();
        public MatchingParameters RsResult { get; set; } = new();
        public MatchingParameters BoResult { get; set; } = new();
    }
}
