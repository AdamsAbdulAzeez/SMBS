using System;

namespace WindowsClient.Shared.UIModels.MaterialBalance
{
    public class ProductionEstimationResultRow
    {
        public static FluidType flowingfluid;
        public DateTime Time { get; set; }
        public double TankPressure { get; set; }
        public double CalculatedFluidProduced { get; set; }
        public double AquiferInflux { get; set; }
        public double F { get; set; }
        public double Bt { get; set; }
        public double Et { get; set; }
        public double Eo { get; set; }
        public double Eg { get; set; }
        public double OilFVF { get; set; }
        public double GasFVF { get; set; }
        public double WaterFVF { get; set; }
        public double OilViscosity { get; set; }
        public double GasViscosity { get; set; }
        public double WaterViscosity { get; set; }
        public double SolutionGOR { get; set; }
        public double ReservoirCGR { get; set; }
        public double WaterCompressibility { get; set; }
        public double FormationCompress { get; set; }
        public double FluidExpansionEnergy { get; set; }
        public double GasCapExpansionEnergy { get; set; }
        public double PVCompressEnergy { get; set; }
        public double WaterInfluxEnergy { get; set; }
        public double GasInjectionEnergy { get; set; }
        public double WaterInjectionEnergy { get; set; }
        public double OilSupportDriveEnergy { get; set; }
        public double GasCapDriveEnergy { get; set; }
        public double SolutionGasDriveEnergy { get; set; }
        public double SupportGasDriveEnergy { get; set; }
        public double NaturalWaterDriveEnergy { get; set; }
        public double CompactionConnateWaterDriveEnergy { get; set; }
        public double ArtificialWaterDriveEnergy { get; set; }
    }
}
