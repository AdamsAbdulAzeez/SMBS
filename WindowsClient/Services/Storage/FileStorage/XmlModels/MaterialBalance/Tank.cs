using System;
using System.Collections.Generic;

namespace WindowsClient.Services.Storage.FileStorage.XmlModels.MaterialBalance
{
    public class Tank
    {
        public Guid Id { get; set; }
        public Guid MaterialBalanceModelId { get; set; }
        public string Name { get; set; }
        public double EstimateDeviation { get; set; }
        public List<ProductionDataRow> BhpData { get; set; }
        public List<ProductionDataRow> SelectedBhpData { get; set; }
        public List<AreaDepthRow> AreaDepthData { get; set; }
        public Aquifer Aquifer { get; set; }
        public Rock Rock { get; set; }
        public FluidType FlowingFluid { get; set; }
        public List<ProductionDataRow> ProductionData { get; set; }
        public List<PvtDataRow> PvtData { get; set; }
        public List<PvtDataRow> PvtMatchingInput { get; set; }
        public RelativePermeabilityData RelPermData { get; set; }
        public double InitialReservoirPressure { get; set; }
        public double BubblePointPressure { get; set; }
        public double DewPointPressure { get; set; }
        public DateTime StartOfProduction { get; set; }
        public double ConnateWaterSaturation { get; set; }
        public List<ProductionEstimationResultRow> ProductionEstimationResult { get; set; }
        public List<PressureSimulationResultRow> PressureSimulationResult { get; set; }
        public PvtInitialCondition PvtInitialCondition { get; set; }
        public List<RegressionResult> RegressionResults { get; set; }
        public RegressionResult AcceptedRegressionResult { get; set; }
        public DateTime Date2Match { get; set; }
        public BoundedVariable STOIP { get; set; }
        public BoundedVariable GIIP { get; set; }
        public BoundedVariable GasCap { get; set; }
        public BoundedVariable Thickness { get; set; }
        public BoundedVariable Width { get; set; }
        public BoundedVariable Length { get; set; }
        public BoundedVariable Radius { get; set; }
        public List<PoreVolumeDepthRow> PoreVolumeDepth { get; set; }
        public bool IsPvd { get; set; }
    }
}
