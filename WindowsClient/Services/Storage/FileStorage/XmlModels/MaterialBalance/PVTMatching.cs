using System.Collections.Generic;

namespace WindowsClient.Services.Storage.FileStorage.XmlModels.MaterialBalance
{
    public class PVTMatching
    {
        public List<MatchingParameters> BubblePointMatching { get; set; } = new List<MatchingParameters>();
        public List<MatchingParameters> SolutionGasMatching { get; set; } = new List<MatchingParameters>();
        public List<MatchingParameters> OilFVFMatching { get; set; } = new List<MatchingParameters>();
        public List<MatchingParameters> OilViscosityMatching { get; set; } = new List<MatchingParameters>();
        public List<MatchingParameters> GasFVFMatching { get; set; } = new List<MatchingParameters>();
        public List<MatchingParameters> GasViscosityMatching { get; set; } = new List<MatchingParameters>();
        //public (PbRsBoModel pbRsBoModel, OilViscosityModel oilViscstyModel,
        //    GasCompressibilityModel gasCompressibilityModel, GasViscosityModel gasViscosityModel) ChoosenModels
        //{ get; set; }
    }
}
