using AutoMapper;
using UIModels = WindowsClient.Shared.UIModels.MaterialBalance;
using EngineShared = SMBS.Shared;

namespace WindowsClient.Services.Storage.Transformations
{
    public class FractionalMatchResultRowProfile : Profile
    {
        public FractionalMatchResultRowProfile()
        {
            CreateMap<UIModels.FractionalMatchResultRow, EngineShared.Result.FractionalMatchResultRow>()
                .ReverseMap();
        }
    }
}
