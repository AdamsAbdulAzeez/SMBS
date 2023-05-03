using AutoMapper;
using WindowsClient.Shared.UIModels.MaterialBalance;

namespace WindowsClient.Services.Storage.Transformations
{
    internal class AquiferConfigurationMapping : Profile
    {
        public AquiferConfigurationMapping()
        {
            CreateMap<AquiferConfiguration, FileStorage.XmlModels.MaterialBalance.AquiferConfiguration>()
                .ReverseMap();
        }
    }
}
