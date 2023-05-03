using AutoMapper;
using WindowsClient.Services.Storage.FileStorage.XmlModels.MaterialBalance;
using UIModels = WindowsClient.Shared.UIModels.MaterialBalance;
using EngineShared = SMBS.Shared;

namespace WindowsClient.Services.Storage.Transformations
{
    public class RelativePermeabilityDataProfile : Profile
    {
        public RelativePermeabilityDataProfile()
        {
            CreateMap<RelativePermeabilityData, UIModels.RelativePermeabilityData>()
                .ReverseMap();

            CreateMap<UIModels.RelativePermeabilityData, EngineShared.DataImport.RelativePermeabilityData>()
                .ReverseMap();
        }
    }
}
