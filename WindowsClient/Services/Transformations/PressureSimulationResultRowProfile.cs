using AutoMapper;
using WindowsClient.Services.Storage.FileStorage.XmlModels.MaterialBalance;
using UIModels = WindowsClient.Shared.UIModels.MaterialBalance;
using EngineShared = SMBS.Shared;

namespace WindowsClient.Services.Storage.Transformations
{
    public class PressureSimulationResultRowProfile : Profile
    {
        public PressureSimulationResultRowProfile()
        {
            CreateMap<PressureSimulationResultRow, UIModels.PressureSimulationResultRow>()
                .ReverseMap();

            CreateMap<UIModels.PressureSimulationResultRow, EngineShared.Result.PressureSimulationResultRow>()
                .ReverseMap();
        }
    }
}
