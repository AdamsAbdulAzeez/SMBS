using System;
using AutoMapper;
using SMBS.Engine;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WindowsClient.Shared.UIModels;
using WindowsClient.Shared.UIModels.MaterialBalance;
using EngineShared = SMBS.Shared;
using MathematicsLibrary2021;
using WindowsClient.Shared.Visualisation;
using WindowsClient.Services.Calculation.PvtMatching;

namespace WindowsClient.Services.Calculator.HistoryMatching
{
    static class EngineExtensions
    {
        public static RegressionVariable AsRegressionVariable(this EngineShared.Variable variable)
        {
            return new RegressionVariable(variable.UpperBound, variable.LowerBound, variable.CurrentValue, "");
        }

        public static EngineShared.Variable AsEngineVariable(this RegressionVariable variable)
        {
            return new EngineShared.Variable
            {
                LowerBound = variable.LowerBound,
                UpperBound = variable.UpperBound,
                CurrentValue = variable.Start,
                TobeOptimized = variable.ToBeOptimized
            };
        }
    }

    internal class HistoryMatchingService : IHistoryMatchingService
    {
        private readonly IMapper _mapper;
        private readonly IPvtMatchingService _pvtMatchingService;

        public HistoryMatchingService(IMapper mapper, IPvtMatchingService pvtMatchingService)
        {
            _mapper = mapper;
            _pvtMatchingService = pvtMatchingService;
        }

        public async Task<EstimateResult> SingleTankEstimateAsync(Tank tank, IChartWrapper chart, XYDataSeries xyDataSeries = null, bool isTurnedOn = false)
        {
            if (xyDataSeries != null)
            {
                if (!isTurnedOn)
                {
                    var indices1 = Enumerable.Range(0, tank.BhpData.Count);
                    chart.SelectedRegressionResult.SelectedData = indices1.ToList();
                }
                for (int i = 0; i < xyDataSeries.Count; i++)
                {
                    var pd = tank.BhpData.FindIndex(p => p.Pressure == xyDataSeries[i].Y && p.OilProduced == xyDataSeries[i].X);
                    if (chart.SelectedRegressionResult.SelectedData.Contains(pd))
                    {
                        chart.SelectedRegressionResult.SelectedData.Remove(pd);
                    }
                    else
                    {
                        chart.SelectedRegressionResult.SelectedData.Add(pd);
                        chart.SelectedRegressionResult.SelectedData.Sort();
                    }
                }
            }

            EngineShared.Tank engineTank = await MapUITankToEngineTank(tank);

            var indices = Enumerable.Range(0, tank.BhpData.Count);
            if (chart.SelectedRegressionResult.SelectedData.Count == 0 || chart.SelectedRegressionResult.SelectedData == null)
                engineTank.SelectedData = indices.ToList();
            else engineTank.SelectedData = chart.SelectedRegressionResult.SelectedData;

            SetUp setUp = new SetUp(engineTank);

            setUp.Estimate();

            var estimatedPressureSeries = new XYDataSeries();

            engineTank.ProductionEstimationResult?.ForEach(row => estimatedPressureSeries.Add((row.CalculatedFluidProduced, row.TankPressure)));
            var selectedSeries = new XYDataSeries();
            var radiusSeries = new List<XYDataSeries>();
            var newWdSeriesAtOtherRadi = new List<NewWdSeriesAtOtherRadi>();

            if (tank.Aquifer.ModelType != WaterInfluxModel.None)
            {

                var isQd = engineTank.Aquifer.WDData?.QD.Count > 0;
                for (int i = 0; i < engineTank.Aquifer.WDData.tDSelected.Count; i++)
                {
                    selectedSeries.Add((Math.Log10(engineTank.Aquifer.WDData.tDSelected[i]), isQd ? engineTank.Aquifer.WDData.QDSelected[i] : engineTank.Aquifer.WDData.PDSelected[i]));
                }

                //TODO: USELESLY COMPLICATED CODE: This can be simplified from the ENGINE API THAT CAN BE SIMPLIFIED FROM THE ENGINE.
                var WDDataAtReservoirRadius = isQd ? engineTank.Aquifer.WDData.QD : engineTank.Aquifer.WDData.PD;


                var count = 0;
                foreach (var ySeries in WDDataAtReservoirRadius)
                {
                    var series = new XYDataSeries
                    {
                        Name = $"R-{engineTank.Aquifer.WDData.rD[count]}"
                    };

                    for (int i = 0; i < ySeries.Count; i++)
                    {
                        series.Add((Math.Log10(engineTank.Aquifer.WDData.tD[i]), ySeries[i]));
                    }
                    radiusSeries.Add(series);


                    // New Series
                    var newSeries = new NewWdSeriesAtOtherRadi
                    {
                        Name = $"R-{engineTank.Aquifer.WDData.rD[count]}"
                    };

                    for (int i = 0; i < ySeries.Count; i++)
                    {
                        newSeries.WdAtOtherRadi.Add((Math.Log10(engineTank.Aquifer.WDData.tD[i]), ySeries[i]));
                    }
                    newWdSeriesAtOtherRadi.Add(newSeries);
                    count++;
                }
            }

            return new EstimateResult
            {
                EstimatedNpSeries = estimatedPressureSeries,
                SelectedTdSeries = selectedSeries,
                WdSeriesAtOtherRadi = radiusSeries.Count > 0 ? radiusSeries.Take(radiusSeries.Count - 1).ToList() : new List<XYDataSeries>(), //Omit the last one
                NewWdSeriesAtOtherRadi = newWdSeriesAtOtherRadi,
                TdSeriesForReservoirRadiusSeries = radiusSeries.Count > 0 ? radiusSeries.Last() : new XYDataSeries(),// The last element always corresponds to the last index from the engine.
                ProductionEstimationResult = _mapper.Map<List<ProductionEstimationResultRow>>(engineTank.ProductionEstimationResult),
            };
        }     

        public async Task<RegressionResult> SingleTankMatchAsync(Tank tank, List<RegressionVariable> variablesToRegress, IChartWrapper chartControl)
        {
            var engineTank = await MapUITankToEngineTank(tank);

            SetRegressionVariables(tank, variablesToRegress, engineTank);

            if (tank.Aquifer.ModelType == WaterInfluxModel.None)
            {
                engineTank.Aquifer = null;
            }

            var indices = Enumerable.Range(0, tank.BhpData.Count);
            if (chartControl.SelectedRegressionResult.SelectedData.Count == 0)
            {
                engineTank.SelectedData = indices.ToList();
                chartControl.SelectedRegressionResult.SelectedData = engineTank.SelectedData;
            }
            else
            {
                engineTank.SelectedData = chartControl.SelectedRegressionResult.SelectedData;
            }
            engineTank.ProductionEstimationResult = new();
            SetUp setUp = new SetUp(engineTank);
            setUp.Tank.PressSimResult = new();

            setUp.SolverChoice = SetUp.SolverType.Levenbergemaquardt;
            setUp.OptimSet = new Optimizers.Set { PopulationSize = 5, Tol = 0.001 };
            await Task.Run(() => setUp.Match());

            var estimatedPressureSeries = new XYDataSeries();

            engineTank.ProductionEstimationResult?.ForEach(row => estimatedPressureSeries.Add((row.CalculatedFluidProduced, row.TankPressure)));

            var selectedSeries = new XYDataSeries();
            var radiusSeries = new List<XYDataSeries>();
            var newWdSeriesAtOtherRadi = new List<NewWdSeriesAtOtherRadi>();

            if (tank.Aquifer.ModelType != WaterInfluxModel.None)
            {

                var isQd = engineTank.Aquifer.WDData?.QD.Count > 0;
                for (int i = 0; i < engineTank.Aquifer.WDData.tDSelected.Count; i++)
                {
                    selectedSeries.Add((Math.Log10(engineTank.Aquifer.WDData.tDSelected[i]), isQd ? engineTank.Aquifer.WDData.QDSelected[i] : engineTank.Aquifer.WDData.PDSelected[i]));
                }

                //TODO: USELESLY COMPLICATED CODE: This can be simplified from the ENGINE API THAT CAN BE SIMPLIFIED FROM THE ENGINE.
                var WDDataAtReservoirRadius = isQd ? engineTank.Aquifer.WDData.QD : engineTank.Aquifer.WDData.PD;


                var count = 0;
                foreach (var ySeries in WDDataAtReservoirRadius)
                {
                    var series = new XYDataSeries
                    {
                        Name = $"R-{engineTank.Aquifer.WDData.rD[count]}"
                    };

                    for (int i = 0; i < ySeries.Count; i++)
                    {
                        series.Add((Math.Log10(engineTank.Aquifer.WDData.tD[i]), ySeries[i]));
                    }
                    radiusSeries.Add(series);


                    // New Series
                    var newSeries = new NewWdSeriesAtOtherRadi
                    {
                        Name = $"R-{engineTank.Aquifer.WDData.rD[count]}"
                    };

                    for (int i = 0; i < ySeries.Count; i++)
                    {
                        newSeries.WdAtOtherRadi.Add((Math.Log10(engineTank.Aquifer.WDData.tD[i]), ySeries[i]));
                    }
                    newWdSeriesAtOtherRadi.Add(newSeries);
                    count++;
                }
            }

            return new RegressionResult
            {
                EstimatedNpSeries = estimatedPressureSeries,
                SelectedTdSeries = selectedSeries,
                SelectedData = engineTank.SelectedData,
                SelectedDataPoint = chartControl.SelectedRegressionResult.SelectedDataPoint,
                WdSeriesAtOtherRadi = radiusSeries.Count > 0 ? radiusSeries.Take(radiusSeries.Count - 1).ToList() : new List<XYDataSeries>(), //Omit the last one
                NewWdSeriesAtOtherRadi = newWdSeriesAtOtherRadi,
                TdSeriesForReservoirRadiusSeries = radiusSeries.Count > 0 ? radiusSeries.Last() : new XYDataSeries(),// The last element always corresponds to the last index from the engine.
                ProductionEstimationResult = _mapper.Map<List<ProductionEstimationResultRow>>(engineTank.ProductionEstimationResult),
                HistoryMatchingVariables = new HistoryMatchingVariables(tank)
                {
                    Thickness = engineTank.Thickness.AsRegressionVariable(),
                    Radius = engineTank.Radius.AsRegressionVariable(),
                    Width = engineTank.Width.AsRegressionVariable(),
                    GasCap = engineTank.GasCap.AsRegressionVariable(),
                    EncroachmentAngle = engineTank.Aquifer != null ? engineTank.Aquifer.EncroachmentAngle.AsRegressionVariable() : tank.Aquifer.EncroachmentAngle.AsRegressionVariable(),
                    OuterInnerRadius = engineTank.Aquifer != null ? engineTank.Aquifer.OuterInnerRadiusRatio.AsRegressionVariable() : tank.Aquifer.OuterInnerRadiusRatio.AsRegressionVariable(),
                    STOIP = engineTank.STOIP.AsRegressionVariable(),
                    Permeability = engineTank.Rock.Permeability.AsRegressionVariable(),
                    Porosity = engineTank.Rock.Porosity.AsRegressionVariable(),
                    Anisotropy = engineTank.Rock.Anisotropy.AsRegressionVariable(),
                    Volume = engineTank.Aquifer != null ? engineTank.Aquifer.Volume.AsRegressionVariable() : tank.Aquifer.Volume.AsRegressionVariable(),
                }
            };
        }

        private async Task<EngineShared.Tank> MapUITankToEngineTank(Tank tank)
        {
            tank.BubblePointPressure.DisplayValue = tank.IsExternalPvt ? tank.PvtData.First().BubblePoint : tank.PvtInitialCondition.BubblePoint;
            tank.DewPointPressure.DisplayValue = tank.IsExternalPvt ? tank.PvtData.First().DewPoint : tank.PvtInitialCondition.DewPoint;

            if (!tank.IsExternalPvt && (tank.MatchedPVT == null || tank.MatchedPVT.PVTGeneratingFunction == null))
            {
                var pvtMatchingService = new PvtMatchingService(_mapper);
                var pvtMatchingInput = pvtMatchingService.GetPvtMatchingInput(tank);
                await pvtMatchingService.MatchPvtAsync(tank, pvtMatchingInput);
            }

            tank.MatchedPVT.ChoosenModels = ((PVTLibrary.Enums.PbRsBoModel)tank.PbRsBoModel,
                (PVTLibrary.Enums.OilViscosityModel)tank.OilViscosityModel, 
                PVTLibrary.Enums.GasCompressibilityModel.HallYarbourough,
                PVTLibrary.Enums.GasViscosityModel.Lee_et_al);

            var engineTank = _mapper.Map<EngineShared.Tank>(tank);

            engineTank.AreaDepthData = tank.AreaDepthData;
            engineTank.PoreVolumeDepth = tank.PoreVolumeDepth;

            if (tank.IsPvd)
                engineTank.AreaDepthData = null;
            else
                engineTank.PoreVolumeDepth = null;

            if (tank.IsExternalPvt) { engineTank.MatchedPVT = null; }

            engineTank.InitialReservoirPressure = tank.InitialReservoirPressure.DisplayValue.Value;
            engineTank.ReservoirTemperature = tank.PvtInitialCondition.Temperature;
            engineTank.Initialise();

            return engineTank;
        }

        private static void SetRegressionVariables(Tank tank, List<RegressionVariable> variablesToRegress, EngineShared.Tank engineTank)
        {
            if (variablesToRegress.Any(x => x.Name == tank.Thickness.Name))
            {
                engineTank.Thickness = variablesToRegress.First(x => x.Name == tank.Thickness.Name).AsEngineVariable();
            }
            else
            {
                engineTank.Thickness.TobeOptimized = false;
            }

            if (variablesToRegress.Any(x => x.Name == tank.Radius.Name))
            {
                engineTank.Radius = variablesToRegress.First(x => x.Name == tank.Radius.Name).AsEngineVariable();
            }
            else
            {
                engineTank.Radius.TobeOptimized = false;
            }

            if (variablesToRegress.Any(x => x.Name == tank.Width.Name))
            {
                engineTank.Width = variablesToRegress.First(x => x.Name == tank.Width.Name).AsEngineVariable();
            }
            else
            {
                engineTank.Width.TobeOptimized = false;
            }

            if (variablesToRegress.Any(x => x.Name == tank.GasCap.Name))
            {
                engineTank.GasCap = variablesToRegress.First(x => x.Name == tank.GasCap.Name).AsEngineVariable();
            }
            else
            {
                engineTank.GasCap.TobeOptimized = false;
            }

            if (variablesToRegress.Any(x => x.Name == tank.Aquifer.EncroachmentAngle.Name))
            {
                engineTank.Aquifer.EncroachmentAngle = variablesToRegress.First(x => x.Name == tank.Aquifer.EncroachmentAngle.Name).AsEngineVariable();
            }
            else
            {
                engineTank.Aquifer.EncroachmentAngle.TobeOptimized = false;
            }

            if (variablesToRegress.Any(x => x.Name == tank.Aquifer.OuterInnerRadiusRatio.Name))
            {
                engineTank.Aquifer.OuterInnerRadiusRatio = variablesToRegress.First(x => x.Name == tank.Aquifer.OuterInnerRadiusRatio.Name).AsEngineVariable();
            }
            else
            {
                engineTank.Aquifer.OuterInnerRadiusRatio.TobeOptimized = false;
            }

            if (variablesToRegress.Any(x => x.Name == tank.STOIP.Name))
            {
                engineTank.STOIP = variablesToRegress.First(x => x.Name == tank.STOIP.Name).AsEngineVariable();
            }
            else
            {
                engineTank.STOIP.TobeOptimized = false;
            }

            if (variablesToRegress.Any(x => x.Name == tank.Rock.Permeability.Name))
            {
                engineTank.Rock.Permeability = variablesToRegress.First(x => x.Name == tank.Rock.Permeability.Name).AsEngineVariable();
            }
            else
            {
                engineTank.Rock.Permeability.TobeOptimized = false;
            }

            if (variablesToRegress.Any(x => x.Name == tank.Rock.Anisotropy.Name))
            {
                engineTank.Rock.Anisotropy = variablesToRegress.First(x => x.Name == tank.Rock.Anisotropy.Name).AsEngineVariable();
            }
            else
            {
                engineTank.Rock.Anisotropy.TobeOptimized = false;
            }

            if (variablesToRegress.Any(x => x.Name == tank.Aquifer.Volume.Name))
            {
                engineTank.Aquifer.Volume = variablesToRegress.First(x => x.Name == tank.Aquifer.Volume.Name).AsEngineVariable();
            }
            else
            {
                engineTank.Aquifer.Volume.TobeOptimized = false;
            }

            if (variablesToRegress.Any(x => x.Name == tank.Rock.Porosity.Name))
            {
                engineTank.Rock.Porosity = variablesToRegress.First(x => x.Name == tank.Rock.Porosity.Name).AsEngineVariable();
            }
            else
            {
                engineTank.Rock.Porosity.TobeOptimized = false;
            }
        }
    }
}
