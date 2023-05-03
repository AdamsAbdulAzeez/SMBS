using AutoMapper;
using UIModels = WindowsClient.Shared.UIModels.MaterialBalance;
using EngineShared = SMBS.Shared;

namespace WindowsClient.Services.Storage.Transformations
{
    public class FractionalMatchResultProfile : Profile
    {
        public FractionalMatchResultProfile()
        {
            CreateMap<UIModels.FractionalMatchResult, EngineShared.Result.FractionalMatchResult>()
                .ReverseMap();
        }
    }
}
