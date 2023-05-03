using AutoMapper;
using PVTLibrary;
using System.Collections.Generic;
using System.Threading.Tasks;
using UiModels = WindowsClient.Shared.UIModels.MaterialBalance;
using System.Linq;
using MathematicsLibrary;
using PVTLibrary.Results;
using System;
using WindowsClient.Shared.UIModels.MaterialBalance;
using PbRsBoModel = PVTLibrary.Enums.PbRsBoModel;
using PvtDataRow = WindowsClient.Shared.UIModels.MaterialBalance.PvtDataRow;

namespace WindowsClient.Services.Calculation.PvtMatching
{
    internal class PvtMatchingService : IPvtMatchingService
    {
        private readonly IMapper _mapper;
        public PvtMatchingService(IMapper mapper) => _mapper = mapper;
         
        public async Task<PvtMatchingResult> MatchAsync(Tank tank)
        {
            PvTMatchingInput matchingInput = GetPvtMatchingInput(tank);
            await MatchPvtAsync(tank, matchingInput);

            var (pbRsBoModel, oilViscstyModel, gasCompressibilityModel, gasViscosityModel) = tank.MatchedPVT.AutoSelect();

            var PbBoRs = Enum.GetValues(typeof(PbRsBoModel));
            var OilVis = Enum.GetValues(typeof(Enums.OilViscosityModel));
            PbRsBoModel oilPbBoRsModel; Enums.OilViscosityModel oilVisModel;

            var generatedPvtData = new List<GeneratedPvt>();
            var pvtres = new PVTResults(tank.MatchedPVT);

            var minPressure = Math.Max(15, matchingInput.Select(d => d.Pressure).Min());
            var maxPressure = matchingInput.Select(d => d.Pressure).Max();
            var numberOfPoints = (int)((maxPressure - minPressure) / 35) + 1;
            var Pressures = ColVec.Linspace(minPressure, maxPressure, numberOfPoints);

            await Task.Run(() =>
            {
                for (int i = 0; i < 7; i++)
                {
                    oilPbBoRsModel = (PbRsBoModel)PbBoRs.GetValue(i);

                    for (int j = 0; j < 6; j++)
                    {
                        oilVisModel = (Enums.OilViscosityModel)OilVis.GetValue(j);

                        var Temperature = matchingInput.BlkOilInput.Temperature;

                        var generatedPvt = new GeneratedPvt
                        {
                            OilViscosityModel = (OilViscosityModel)oilVisModel,
                            PbRsBoModel = (UiModels.PbRsBoModel)oilPbBoRsModel,
                        };
                        for (int l = 0; l < Pressures.Numel; l++)
                        {
                            pvtres.GetPvtResult(matchingInput.BlkOilInput, oilPbBoRsModel, oilVisModel, Pressures[l], Temperature,
                                gasCompressibilityModel, gasViscosityModel);

                            PvtDataRow pvtDataRow = new PvtDataRow
                            {
                                BubblePoint = pvtres.Pb,
                                GasDensity = pvtres.RhoG,
                                OilDensity = pvtres.RhoO,
                                WaterDensity = pvtres.RhoW,
                                GasFVF = pvtres.Bg,
                                GasOilRatio = pvtres.Rs,
                                OilFVF = pvtres.Bo,
                                ZFactor = pvtres.Zfactor,
                                WaterFVF = pvtres.Bw,
                                GasViscosity = pvtres.Mug,
                                OilViscosity = pvtres.Muo,
                                WaterViscosity = pvtres.Muw,
                                Temperature = pvtres.Temp,
                                Pressure = pvtres.Press,
                            };
                            generatedPvt.Data.Add(pvtDataRow);
                        }
                        generatedPvtData.Add(generatedPvt);
                    }
                }
            });


            return new PvtMatchingResult
            {
                BubblePointMatchingResult = _mapper.Map<List<MatchingParameters>>(tank.MatchedPVT.BubblePointMatching),
                SolutionGasMatchingResult = _mapper.Map<List<MatchingParameters>>(tank.MatchedPVT.SolutionGasMatching),
                OilFVFMatchingResult = _mapper.Map<List<MatchingParameters>>(tank.MatchedPVT.OilFVFMatching),
                OilViscosityMatchingResult = _mapper.Map<List<MatchingParameters>>(tank.MatchedPVT.OilViscosityMatching),
                GasFVFMatchingResult = _mapper.Map<List<MatchingParameters>>(tank.MatchedPVT.GasFVFMatching),
                GasViscosityMatchingResult = _mapper.Map<List<MatchingParameters>>(tank.MatchedPVT.GasViscosityMatching),
                AutoSelected = ((UiModels.PbRsBoModel)pbRsBoModel,
                                    (OilViscosityModel)oilViscstyModel),
                GeneratedPvtData = generatedPvtData
            };
        }

        internal PvTMatchingInput GetPvtMatchingInput(Tank tank)
        {
            var pvtData = _mapper.Map<List<PVTLibrary.PvtDataRow>>(tank.PvtMatchingInput);

            PvTMatchingInput matchingInput = new PvTMatchingInput();
            matchingInput.AddRange(pvtData);

            matchingInput.BlkOilInput = new BlackOilInput
            {
                Rs1 = tank.PvtInitialCondition.GOR,
                API = tank.PvtInitialCondition.OilGravity,
                Gg1 = tank.PvtInitialCondition.GasGravity,
                Salinity = tank.PvtInitialCondition.WaterSalinity,
                MoleH2S = tank.PvtInitialCondition.MoleH2S,
                MoleCO2 = tank.PvtInitialCondition.MoleCO2,
                MoleN2 = tank.PvtInitialCondition.MoleN2,
                Temperature = tank.PvtInitialCondition.Temperature,
                LabBubblePoint = tank.PvtInitialCondition.BubblePoint,
            };

            matchingInput.BlkOilInput.Tsp1 = tank.SeparatorConfiguration.Tsep1;
            matchingInput.BlkOilInput.Psp1 = tank.SeparatorConfiguration.Psep1;
            matchingInput.BlkOilInput.Tsp2 = tank.SeparatorConfiguration.Tsep2;
            matchingInput.BlkOilInput.Psp2 = tank.SeparatorConfiguration.Psep2;
            matchingInput.BlkOilInput.Tsp3 = tank.SeparatorConfiguration.Tsep3;
            matchingInput.BlkOilInput.Psp3 = tank.SeparatorConfiguration.Psep3;
            matchingInput.BlkOilInput.SepConfig = (Enums.SeparatorStage)tank.SeparatorConfiguration.SeparatorStage;

            return matchingInput;
        }

        internal async Task MatchPvtAsync(Tank tank, 
            PvTMatchingInput matchingInput, 
            PVTMatching.MatchingMethod matchingMethod = PVTMatching.MatchingMethod.EndPoint)
        {
            await Task.Run(() => tank.MatchedPVT.Match(matchingInput, matchingMethod));

            var (pbRsBoModel, oilViscstyModel, gasCompressibilityModel, gasViscosityModel) = tank.MatchedPVT.AutoSelect();
        }
    }
}
