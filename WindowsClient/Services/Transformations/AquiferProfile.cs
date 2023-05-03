using AutoMapper;
using UIModels = WindowsClient.Shared.UIModels.MaterialBalance;
using EngineShared = SMBS.Shared;
using WindowsClient.Services.Storage.FileStorage.XmlModels.MaterialBalance;

namespace WindowsClient.Services.Storage.Transformations
{
    public class AquiferProfile : Profile
    {
        public AquiferProfile()
        {
            CreateMap<Aquifer, UIModels.Aquifer>()
                .ReverseMap();

            CreateMap<UIModels.Aquifer, EngineShared.Aquifer>()
                .ReverseMap();
        }
    }

    public class AquiferConfigurationProfile : Profile
    {
        public AquiferConfigurationProfile()
        {
            CreateMap<UIModels.AquiferConfiguration, EngineShared.Aquifer.AquiferConfiguration>()
                .ReverseMap();
        }
    }
}
