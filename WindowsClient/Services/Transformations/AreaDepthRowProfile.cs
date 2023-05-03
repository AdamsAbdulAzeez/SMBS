using AutoMapper;
using WindowsClient.Services.Storage.FileStorage.XmlModels.MaterialBalance;
using UIModels = WindowsClient.Shared.UIModels.MaterialBalance;
using EngineShared = SMBS.Shared;

namespace WindowsClient.Services.Storage.Transformations
{
    public class AreaDepthRowProfile : Profile
    {
        public AreaDepthRowProfile()
        {
            CreateMap<AreaDepthRow, UIModels.AreaDepthRow>()
                .ReverseMap();

            CreateMap<AreaDepthRow, EngineShared.DataImport.AreaDepthRow>()
                .ReverseMap();
        }
    }
}
