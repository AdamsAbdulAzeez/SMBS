using System.Collections.Generic;

namespace WindowsClient.Shared.UIModels.MaterialBalance
{
    public class GeneratedPvt
    {
        public PbRsBoModel PbRsBoModel { get; set; }
        public OilViscosityModel OilViscosityModel { get; set; }
        public List<PvtDataRow> Data { get; set; } = new();
    }
}
