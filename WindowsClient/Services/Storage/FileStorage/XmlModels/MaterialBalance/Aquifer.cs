using System.Collections.Generic;

namespace WindowsClient.Services.Storage.FileStorage.XmlModels.MaterialBalance
{
    public class Aquifer
    {
        public BoundedVariable OuterInnerRadiusRatio { get; set; } = new();
        public BoundedVariable EncroachmentAngle { get; set; } = new();
        public AquiferConfiguration Configuration { get; set; } = new();
        public BoundedVariable Volume { get; set; } = new();
        public WaterInfluxModel ModelType { get; set; }
        public WaterInfluxState StateType { get; set; }
        public BoundaryCondition BoundaryType { get; set; }
    }

    public enum WaterInfluxModel
    {
        None,
        Hurst_Dake,
        Hurst_Modified,
        CaterTracy,
        Fetkovich
    }

    public enum WaterInfluxState
    {
        Steady,
        PsuedoSteady
    }
    public enum Geometry
    {
        Linear,
        Radial
    }
    public enum Position
    {
        Edge,
        Bottom
    }
    public enum BoundaryCondition
    {
        Closed_Boundary,
        Constant_Pressure_Boundary,
        Infinite_Acting
    }
}
