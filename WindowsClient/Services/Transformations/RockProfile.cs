using AutoMapper;
using WindowsClient.Services.Storage.FileStorage.XmlModels.MaterialBalance;
using UIModels = WindowsClient.Shared.UIModels.MaterialBalance;
using EngineShared = SMBS.Shared;

namespace WindowsClient.Services.Storage.Transformations
{
    public class RockProfile : Profile
    {
        public RockProfile()
        {
            CreateMap<Rock, UIModels.Rock>()
                .ReverseMap();

            CreateMap<UIModels.Rock, EngineShared.Rock>();
        }
    }
}
