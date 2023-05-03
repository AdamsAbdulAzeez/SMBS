using AutoMapper;
using WindowsClient.Shared.UIModels.MaterialBalance;

namespace WindowsClient.Services.Storage.Transformations
{
    internal class RegressionProfile : Profile
    {
        public RegressionProfile()
        {
            CreateMap<RegressionVariable, SMBS.Shared.Variable>()
                .ReverseMap();
        }
    }
}
