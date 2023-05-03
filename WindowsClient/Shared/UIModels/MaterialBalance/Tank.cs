using System;
using System.Collections.Generic;
using System.Linq;
using CypherCrescent.Units.Services;
using CypherCrescent.Units.Variables;
using Prism.Mvvm;
using PVTLibrary;
using WindowsClient.Shared.ErrorHandling;
using EngineShared = SMBS.Shared.DataImport;
using static System.Math;

namespace WindowsClient.Shared.UIModels.MaterialBalance
{
    public class Tank : BindableBase
    {
        public Tank()
        {
            Aquifer = new Aquifer();
            Rock = new Rock();
            Id = Guid.NewGuid();

            SetUnits();
        }

        private void SetUnits()
        {
            var unitService = Ioc.Resolve<IUnitService>();

            unitService.RegisterVariable(STOIP.CurrentValue);
            unitService.RegisterVariable(GIIP.CurrentValue);
            unitService.RegisterVariable(Thickness.CurrentValue);
            unitService.RegisterVariable(Width.CurrentValue);
            unitService.RegisterVariable(Length.CurrentValue);
            unitService.RegisterVariable(Radius.CurrentValue);
            unitService.RegisterVariable(InitialReservoirPressure);
            unitService.RegisterVariable(BubblePointPressure);
            unitService.RegisterVariable(DewPointPressure);
            unitService.RegisterVariable(ConnateWaterSaturation);
            unitService.RegisterVariable(EstimateDeviation);

            unitService.RegisterVariable(Rock.Porosity.CurrentValue);
        }

        public XYDataSeries GetPressureVsCummulativeProduction()
        {
            var series = new XYDataSeries(true, true);
            if (FlowingFluid == FluidType.Oil)
                ProductionData.ForEach(row => series.Add((row.OilProduced, row.Pressure)));
            else
                ProductionData.ForEach(row => series.Add((row.GasProduced, row.Pressure)));
            return series;
        }

        public TimeSeries GetSelectedBhpData()
        {
            var series = new TimeSeries(true);
            BhpData.ForEach(row => series.Add((row.Time, row.Pressure)));
            return series;
        }

        public TimeSeries GetPressureData()
        {
            var series = new TimeSeries(true);
            BhpData.ForEach(row => series.Add((row.Time, row.Pressure)));
            return series;
        }

        public TimeSeries GetCummulativeOilProducedData()
        {
            var series = new TimeSeries(true);
            BhpData.ForEach(row => series.Add((row.Time, row.OilProduced)));
            return series;
        }

        public TimeSeries GetCummulativeGasProducedData()
        {
            var series = new TimeSeries(true);
            BhpData.ForEach(row => series.Add((row.Time, row.GasProduced)));
            return series;
        }

        public TimeSeries GetCummulativeGasInjectedData()
        {
            var series = new TimeSeries(true);
            BhpData.ForEach(row => series.Add((row.Time, row.GasInjected)));
            return series;
        }

        public TimeSeries GetCummulativeWaterInjectedData()
        {
            var series = new TimeSeries(true);
            BhpData.ForEach(row => series.Add((row.Time, row.WaterInjected)));
            return series;
        }

        public TimeSeries GetCummulativeWaterProducedData()
        {
            var series = new TimeSeries(true);
            BhpData.ForEach(row => series.Add((row.Time, row.WaterProduced)));
            return series;
        }

        public void SetTankAquifer(Aquifer aquifer)
        {
            this.Aquifer = aquifer;
            TankAquiferChanged?.Invoke();
        }

        public void SetPvtMatchingInput(List<PvtDataRow> pvtData)
        {
            PvtMatchingInput = pvtData;
            PvtMatchingInputChanged?.Invoke();
        }

        public void CalculateRelativePermeabilityPlotData()
        {
            double Krop, Krwp, Krgp, no, nw, ng, Swc, Sor, Sgr;
            Swc = RelPermData.WaterRelPerm.ResidualSaturation;
            Sor = RelPermData.OilRelPerm.ResidualSaturation;
            Sgr = RelPermData.GasRelPerm.ResidualSaturation;
            Krwp = RelPermData.WaterRelPerm.EndPoint;
            Krop = RelPermData.OilRelPerm.EndPoint;
            Krgp = RelPermData.GasRelPerm.EndPoint;
            nw = RelPermData.WaterRelPerm.Exponent;
            no = RelPermData.OilRelPerm.Exponent;
            ng = RelPermData.GasRelPerm.Exponent;
            double DS = (1 - Swc - Sor - Sgr);

            double Krofunc(double so) => Krop * Pow((so - Sor) / DS, no);
            double Krwfunc(double sw) => Krwp * Pow((sw - Swc) / DS, nw);
            double Krgfunc(double sg) => Krgp * Pow((sg - Sgr) / DS, ng);

            double ds = DS / 20, sok = Sor, swk = Swc, sgk = Sgr;
            RelativePermeabilityPlot = new RelativePermeabilityPlot();
            for (int i = 0; i <= 20; i++)
            {
                RelativePermeabilityPlot.So.Add(sok); RelativePermeabilityPlot.Kro.Add(Krofunc(sok));
                RelativePermeabilityPlot.Sw.Add(swk); RelativePermeabilityPlot.Krw.Add(Krwfunc(swk));
                RelativePermeabilityPlot.Sg.Add(sgk); RelativePermeabilityPlot.Krg.Add(Krgfunc(sgk));
                sok += ds; sgk += ds; swk += ds;
            }
        }

        public Guid Id { get; set; }
        public Guid ModelId { get; set; }
        public string Name { get; set; }

        public PvtInitialCondition PvtInitialCondition { get; set; } = new();
        public RelativePermeabilityData RelPermData { get; set; } = new();
        public List<int> SelectedData { get; set; } = new();
        public List<ProductionDataRow> BhpData => ProductionData
            .Where(row => row.Pressure != 0)
            .ToList();

        public List<ProductionDataRow> SelectedBhpData => BhpData.Where(row => row.IsSelected).ToList();
        public EngineShared.AreaDepthData AreaDepthData { get; set; } = new();
        public EngineShared.PoreVolumeDepth PoreVolumeDepth { get; set; } = new();
        public List<ProductionDataRow> ProductionData { get; set; } = new();
        public PressureSimulationResultData PressureSimulationData { get; set; } = new();
        public List<RegressionResult> RegressionResults { get; set; } = new();
        public RegressionResult AcceptedRegressionResult { get; set; }
        public List<PvtDataRow> PvtData { get; set; } = new();
        public List<PvtDataRow> PvtMatchingInput { get; private set; } = new();
        public BoundedVariable<OriginalOilInPlace> STOIP { get; set; } = new();
        public BoundedVariable<InitialGasInPlace> GIIP { get; set; } = new();
        public BoundedVariable<Dimensionless> GasCap { get; set; } = new();
        public BoundedVariable<ReservoirThickness> Thickness { get; set; } = new();
        public BoundedVariable<ReservoirWidth> Width { get; set; } = new();
        public BoundedVariable<ReservoirLength> Length { get; set; } = new();
        public BoundedVariable<ReservoirRadius> Radius { get; set; } = new();
        public LabPVT LabPvt { get; set; } = new();
        public ReservoirPressure InitialReservoirPressure { get; set; } = new();
        public ReservoirPressure BubblePointPressure { get; set; } = new();
        public ReservoirPressure DewPointPressure { get; set; } = new();
        public ConnateWaterSaturation ConnateWaterSaturation { get; set; } = new();
        public ReservoirDeviationAngle EstimateDeviation { get; set; } = new();
        public SeparatorConfiguration SeparatorConfiguration { get; set; } = new();
        public PVTMatching MatchedPVT { get; set; } = new();
        public RelativePermeabilityPlot RelativePermeabilityPlot { get; set; } = new();

        private FractionalMatchResult fractionalMatchResult = new();
        public FractionalMatchResult FractionalMatchResult
        {
            get => fractionalMatchResult; 
            set
            {
                fractionalMatchResult = value;
                FractionalMatchResultChanged?.Invoke();
            }
        }
        public bool IsExternalPvt { get; set; }
        public DateTime StartOfProduction { get; set; }
        public Aquifer Aquifer { get; private set; }
        public Rock Rock { get; set; }
        private FluidType flowingFluid;
        public FluidType FlowingFluid
        {
            get => flowingFluid;
            set
            {
                flowingFluid = value;
                FlowingFluidChanged?.Invoke(value);
            }
        }

        public OilViscosityModel OilViscosityModel { get; set; }
        public PbRsBoModel PbRsBoModel { get; set; }
        public bool IsPvd { get; set; }
        internal event Action<FluidType> FlowingFluidChanged;
        internal event Action TankAquiferChanged;
        internal event Action PvtMatchingInputChanged;
        internal event Action FractionalMatchResultChanged;

        internal IActionResult Validate() { throw new NotImplementedException(); }
        public double UserCompressibility = 0;

        public double EvaluateCompressibility(double Porosity)
        {
            if (UserCompressibility != 0)
                return UserCompressibility;
            else
            {
                double compressibility = 3.2e-6;
                if (Porosity < 0.3) compressibility += Pow(0.3 - Porosity, 2.415) * 7.8e-5;
                return compressibility;
            }
        }
    }
}
