using System;

namespace WindowsClient.Services.Storage.FileStorage.XmlModels.MaterialBalance
{
    public class PressureSimulationResultRow
    {
        public static FluidType flowingfluid;
        public DateTime Time { get; set; }
        public double TankPressure { get; set; }
        public double HistoryPressure { get; set; }
        public double OilRecoveryFactor { get; set; }
        public double GasRecoveryFactor { get; set; }
        public double AverageOilRate { get; set; }
        public double AverageGasRate { get; set; }
        public double AverageWaterRate { get; set; }
        public double AverageLiquidRate { get; set; }
        public double AverageGasInjectionRate { get; set; }
        public double AverageWaterInjectionRate { get; set; }
        public double P_Z { get; set; }
        public double OilSaturation { get; set; }
        public double GasSaturation { get; set; }
        public double WaterSaturation { get; set; }
        public double OilFVF { get; set; }
        public double GasFVF { get; set; }
        public double WaterFVF { get; set; }
        public double OilViscosity { get; set; }
        public double GasViscosity { get; set; }
        public double WaterViscosity { get; set; }
        public double OilDensity { get; set; }
        public double GasDensity { get; set; }
        public double WaterDensity { get; set; }
        public double ZFactor { get; set; }
        public double VaporisedCGR { get; set; }
        public double WGR { get; set; }
        public double VaporisedWGR { get; set; }
        public double WaterCompressiblity { get; set; }
        public double AquiferInflux { get; set; }
        public double RockCompressiblity { get; set; }
        public double CummulativeGasProduced { get; set; }
        public double CummulativeOilProduced { get; set; }
        public double CummulativeWaterProduced { get; set; }
        public double CummulativeGasInjected { get; set; }
        public double CummulativeWaterInjected { get; set; }
        public double PoreVolume { get; set; }
        public double OilWaterContact { get; set; }
        public double GasOilContact { get; set; }
        public double GasWaterContact { get; set; }
        public double WaterCut { get; set; }
        public double SolutionGOR { get; set; }
        public double ReservoirCGR { get; set; }
        public double ProducingGOR { get; set; }
        public double ProducingCGR { get; set; }
        public double CummulativeGOR { get; set; }
        public double CummulativeCGR { get; set; }
    }
}
