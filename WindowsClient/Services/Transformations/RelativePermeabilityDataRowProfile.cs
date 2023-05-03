using AutoMapper;
using WindowsClient.Services.Storage.FileStorage.XmlModels.MaterialBalance;
using UIModels = WindowsClient.Shared.UIModels.MaterialBalance;
using EngineShared = SMBS.Shared;

namespace WindowsClient.Services.Storage.Transformations
{
    public class RelativePermeabilityDataRowProfile : Profile
    {
        public RelativePermeabilityDataRowProfile()
        {
            CreateMap<RelativePermeabilityDataRow, UIModels.RelativePermeabilityDataRow>()
                .ReverseMap();

            CreateMap<UIModels.RelativePermeabilityDataRow, EngineShared.DataImport.RelativePermeabilityDataRow>()
                .ReverseMap();
        }
    }
}
