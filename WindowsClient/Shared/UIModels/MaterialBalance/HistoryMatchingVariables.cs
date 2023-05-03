using System.Collections.Generic;
using System.Xml.Serialization;
using Prism.Mvvm;

namespace WindowsClient.Shared.UIModels.MaterialBalance
{
    public class HistoryMatchingVariables : BindableBase
    {
        private HistoryMatchingVariables() { }
        public HistoryMatchingVariables(Tank tank)
        {
            Tank = tank;

            Thickness = Tank.Thickness.AsRegressionVariable();
            Radius = Tank.Radius.AsRegressionVariable();
            Permeability = Tank.Rock.Permeability.AsRegressionVariable();
            Porosity = Tank.Rock.Porosity.AsRegressionVariable();
            Anisotropy = Tank.Rock.Anisotropy.AsRegressionVariable();
            STOIP = Tank.STOIP.AsRegressionVariable();
            OuterInnerRadius = Tank.Aquifer.OuterInnerRadiusRatio.AsRegressionVariable();
            EncroachmentAngle = Tank.Aquifer.EncroachmentAngle.AsRegressionVariable();
            Volume = Tank.Aquifer.Volume.AsRegressionVariable();
            Width = Tank.Width.AsRegressionVariable();
            GasCap = Tank.GasCap.AsRegressionVariable();
            Length = Tank.Length.AsRegressionVariable();
            GIIP = Tank.GIIP.AsRegressionVariable();
            List = GetRegressionVariables();
            SetDisplayNames();
        }

        private void SetDisplayNames()
        {
            OuterInnerRadius.DisplayName = "Outer Inner Radius";
            Thickness.DisplayName = "Reservoir Thickness";
            Radius.DisplayName = "Reservoir Radius";
            STOIP.DisplayName = "Original Oil In Place";
            EncroachmentAngle.DisplayName = "Encroachment Angle";
            Permeability.DisplayName = "Reservoir Permeability";
        }

        public void SetBestFits(HistoryMatchingVariables resultVariables)
        {
            Thickness.BestFitValue = resultVariables.Thickness.BestFitValue;
            Radius.BestFitValue = resultVariables.Radius.BestFitValue;
            Permeability.BestFitValue = resultVariables.Permeability.BestFitValue;
            Porosity.BestFitValue = resultVariables.Porosity.BestFitValue;
            STOIP.BestFitValue = resultVariables.STOIP.BestFitValue;
            OuterInnerRadius.BestFitValue = resultVariables.OuterInnerRadius.BestFitValue;
            EncroachmentAngle.BestFitValue = resultVariables.EncroachmentAngle.BestFitValue;
            Volume.BestFitValue = resultVariables.Volume.BestFitValue;
            Width.BestFitValue = resultVariables.Width.BestFitValue;
            GasCap.BestFitValue = resultVariables.GasCap.BestFitValue;
            Anisotropy.BestFitValue = resultVariables.Anisotropy.BestFitValue;
            GIIP.BestFitValue = resultVariables.GIIP.BestFitValue;
            Length.BestFitValue = resultVariables.Length.BestFitValue;
        }

        internal List<RegressionVariable> GetRegressionVariables()
        {
            // TODO: Add Porosity to variables in tank
            List<RegressionVariable> list = new List<RegressionVariable>();
            //var list = new List<RegressionVariable> { Thickness, Permeability, Porosity };
            switch (FlowingFluid)
            {
                case FluidType.Oil:
                    list.Add(STOIP);
                    break;
                case FluidType.Gas:
                    list.Add(GIIP);
                    break;
                case FluidType.Condensate:
                    list.Add(GIIP);
                    break;
                default:
                    break;
            }
            list.Add(Thickness);
            list.Add(Permeability);
            list.Add(Porosity);
            if (IsRadial)
            {
                list.Add(Radius);
                list.Add(OuterInnerRadius);
                list.Add(EncroachmentAngle);
            }            
            if (IsLinear)
            {
                list.Add(Volume);
                list.Add(Width);
                list.Add(GasCap);
                list.Add(Length);
            }

            if (IsBottom)
            {
                list.Add(Anisotropy);
            }
            return list;
        }

        public bool IsRadial => Tank.Aquifer.Configuration.Geometry == Geometry.Radial;
        public bool IsLinear => Tank.Aquifer.Configuration.Geometry == Geometry.Linear;
        public bool IsBottom => Tank.Aquifer.Configuration.Position == Position.Bottom;
        public FluidType FlowingFluid => Tank.FlowingFluid;
        [XmlIgnore]
        public Tank Tank { get; set; }
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