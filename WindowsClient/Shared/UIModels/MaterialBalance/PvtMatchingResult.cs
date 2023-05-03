using System.Collections.Generic;
using System.Linq;

namespace WindowsClient.Shared.UIModels.MaterialBalance
{
    public class PvtMatchingResult
    {
        public List<MatchingParameters> BubblePointMatchingResult { get; internal set; } = new();
        public List<MatchingParameters> SolutionGasMatchingResult { get; internal set; } = new();
        public List<MatchingParameters> OilFVFMatchingResult { get; internal set; } = new();
        public List<MatchingParameters> OilViscosityMatchingResult { get; internal set; } = new();
        public List<MatchingParameters> GasFVFMatchingResult { get; internal set; } = new();
        public List<MatchingParameters> GasViscosityMatchingResult { get; internal set; } = new();
        public List<GeneratedPvt> GeneratedPvtData { get; internal set; } = new();

        public MatchingParameters GetPbResultFor(PbRsBoModel pbRsBoModel) =>
            BubblePointMatchingResult.FirstOrDefault(r => r.Name == pbRsBoModel.ToString());

        public MatchingParameters GetRsResultFor(PbRsBoModel pbRsBoModel) =>
            SolutionGasMatchingResult.FirstOrDefault(r => r.Name == pbRsBoModel.ToString());

        public MatchingParameters GetBoResultFor(PbRsBoModel pbRsBoModel) =>
            OilFVFMatchingResult.FirstOrDefault(r => r.Name == pbRsBoModel.ToString());

        public MatchingParameters GetUoResultFor(OilViscosityModel oilViscosityModel) =>
            OilViscosityMatchingResult.FirstOrDefault(r => r.Name == oilViscosityModel.ToString());

        public MatchingParameters GetBgResultFor(GasCompressibilityModel GasCompressibilityModel = GasCompressibilityModel.HallYarbourough) =>
            GasFVFMatchingResult.FirstOrDefault(r => r.Name == GasCompressibilityModel.ToString());

        public MatchingParameters GetGasViscosityResultFor(GasViscosityModel gasViscosityModel = GasViscosityModel.Lee_et_al) =>
            GasViscosityMatchingResult.FirstOrDefault(r => r.Name == gasViscosityModel.ToString());

        public (PbRsBoModel pbRsBoModel, OilViscosityModel oilViscosityModel) AutoSelected { get; set; }

        public PbRsBoCorrelationResult GetPbRsBoCorrelationResult(PbRsBoModel pbRsBoModel)
        {
            return new PbRsBoCorrelationResult
            {
                BoResult = GetBoResultFor(pbRsBoModel),
                PbResult = GetPbResultFor(pbRsBoModel),
                RsResult = GetRsResultFor(pbRsBoModel)
            };
        }
    }
}
