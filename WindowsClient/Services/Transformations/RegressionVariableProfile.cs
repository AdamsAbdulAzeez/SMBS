using AutoMapper;
using WindowsClient.Services.Storage.FileStorage.XmlModels.MaterialBalance;
using UIModels = WindowsClient.Shared.UIModels.MaterialBalance;

namespace WindowsClient.Services.Storage.Transformations
{
    public class RegressionVariableProfile : Profile
    {
        public RegressionVariableProfile()
        {
            CreateMap<RegressionVariable, UIModels.RegressionVariable>()
                .ReverseMap();
        }
    }
}
