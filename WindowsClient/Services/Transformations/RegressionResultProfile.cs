using AutoMapper;
using WindowsClient.Services.Storage.FileStorage.XmlModels.MaterialBalance;
using UIModels = WindowsClient.Shared.UIModels.MaterialBalance;

namespace WindowsClient.Services.Storage.Transformations
{
    public class RegressionResultProfile : Profile
    {
        public RegressionResultProfile()
        {
            CreateMap<RegressionResult, UIModels.RegressionResult>()
                .ReverseMap();
        }
    }
}
