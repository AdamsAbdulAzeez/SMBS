using AutoMapper;
using SMBS.Engine;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WindowsClient.Shared.UIModels.MaterialBalance;
using EngineShared = SMBS.Shared;

namespace WindowsClient.Services.Calculator.PressureSimulation
{
    internal class PressureSimulationService : IPressureSimulationService
    {
        private readonly IMapper _mapper;
        public PressureSimulationService(IMapper mapper) => _mapper = mapper;

        public async Task<List<PressureSimulationResultRow>> SimulateAsync(Tank tank, DateUpdate dateUpdate, int step)
        {
            var engineTank = _mapper.Map<EngineShared.Tank>(tank);

            engineTank.AreaDepthData = tank.AreaDepthData;
            engineTank.PoreVolumeDepth = tank.PoreVolumeDepth;

            if (tank.IsPvd)
                engineTank.AreaDepthData = null;
            else
                engineTank.PoreVolumeDepth = null;

            engineTank.InitialReservoirPressure = tank.InitialReservoirPressure.DisplayValue.Value;
            engineTank.ReservoirTemperature = tank.PvtInitialCondition.Temperature;
            engineTank.Initialise();
            engineTank.BubblePointPressure = tank.PvtInitialCondition.BubblePoint;

            SetUp setUp = new SetUp(engineTank);

            var indices = Enumerable.Range(0, tank.BhpData.Count);
            engineTank.SelectedData = indices.ToList();

            //TODO: set this from Tank's IsMatched property
            setUp.isMatched = true;
            //engineTank.ProductionEstimationResult = new();
            //setUp.Tank.PressSimResult = new();
            await Task.Run(() => setUp.Simulate(step, (SetUp.DateUpdate)dateUpdate));

            var result = _mapper.Map<List<PressureSimulationResultRow>>(setUp.Tank.PressSimResult);
            tank.FractionalMatchResult = _mapper.Map<FractionalMatchResult>(engineTank.FractionalMatchResult);

            FillHistoryPressure(setUp, result, tank);
            return result;
        }

        private static void FillHistoryPressure(SetUp setUp, List<PressureSimulationResultRow> result, Tank tank)
        {
            var productionDataDictionary = setUp.Tank.PressSimResult.ToDictionary(row => (row.Time.Month, row.Time.Year));
            
            var bhpDataDictionary = tank.BhpData.Where(row => productionDataDictionary.ContainsKey((row.Time.Month, row.Time.Year)))
                .ToDictionary(row => (row.Time.Month, row.Time.Year));

            int i = 0;
            foreach (var key in productionDataDictionary.Keys)
            {
                result[i].HistoryPressure = bhpDataDictionary.ContainsKey(key)
                    ? bhpDataDictionary[key].Pressure : 0;
                i++;
            }
        }
    }
}
