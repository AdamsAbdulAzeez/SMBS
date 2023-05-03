using WindowsClient.Shared.EnumBinding;
using System.Collections.Generic;
using System.ComponentModel;
using System;
using CypherCrescent.Units.Variables;

namespace WindowsClient.Shared.UIModels.MaterialBalance
{
    public class Aquifer
    {
        public Aquifer()
        {
            OuterInnerRadiusRatio = new BoundedVariable<OuterInnerRadiusRatio>();
            EncroachmentAngle = new BoundedVariable<EncroachmentAngle>();
            Volume = new BoundedVariable<Volume>();
            Anisotropy = new BoundedVariable<Dimensionless>();
            Configuration = new AquiferConfiguration();
        }
        public BoundedVariable<OuterInnerRadiusRatio> OuterInnerRadiusRatio { get; set; }
        public BoundedVariable<EncroachmentAngle> EncroachmentAngle { get; set; }
        public BoundedVariable<Volume> Volume { get; set; }
        public BoundedVariable<Dimensionless> Anisotropy { get; set; }
        public WaterInfluxState StateType { get; set; }
        public AquiferConfiguration Configuration { get; set; }
        public BoundaryCondition BoundaryType { get; set; }

        private WaterInfluxModel modelType;
        public WaterInfluxModel ModelType
        {
            get { return modelType; }
            set
            {
                modelType = value;
                ModelTypeChanged?.Invoke();
            }
        }
        internal event Action ModelTypeChanged;

    }

    public class AquiferConfiguration
    {
        private Geometry geometry;

        public Geometry Geometry
        {
            get { return geometry; }
            set
            {
                geometry = value;
                GeometryChanged?.Invoke();
            }
        }
        private Position position;
        public Position Position
        {
            get => position;
            set
            {
                position = value;
                PositionChanged?.Invoke();
            }
        }
        internal event Action GeometryChanged;
        internal event Action PositionChanged;
    }

    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum WaterInfluxModel
    {
        None,
        [Description("Hurst Dake")]
        Hurst_Dake,
        [Description("Hurst Modified")]
        Hurst_Modified,
        [Description("Carter Tracy")]
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
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum BoundaryCondition
    {
        [Description("Closed Boundary")]
        Closed_Boundary,
        [Description("Constant Pressure Boundary")]
        Constant_Pressure_Boundary,
        [Description("Infinite Acting")]
        Infinite_Acting
    }
}
