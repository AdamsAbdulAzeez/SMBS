using System.Collections.Generic;

namespace WindowsClient.Services.Storage.FileStorage.XmlModels.MaterialBalance
{
    public class HistoryMatchingVariables 
    {
        public bool IsRadial { get; set; }
        public bool IsLinear { get; set; }
        public bool IsBottom { get; set; }
        public FluidType FlowingFluid { get; set; }
        public RegressionVariable Thickness { get; set; }
        public RegressionVariable Radius { get; set; }
        public RegressionVariable OuterInnerRadius { get; set; }
        public RegressionVariable EncroachmentAngle { get; set; }
        public RegressionVariable Permeability { get; set; }
        public RegressionVariable Porosity { get; set; }
        public RegressionVariable STOIP { get; set; }
        public RegressionVariable Volume { get; set; }
        public RegressionVariable Width { get; set; }
        public RegressionVariable GasCap { get; set; }
        public RegressionVariable Anisotropy { get; set; }
        public RegressionVariable GIIP { get; set; }
        public RegressionVariable Length { get; set; }
        public List<RegressionVariable> List { get; }
    }
}
