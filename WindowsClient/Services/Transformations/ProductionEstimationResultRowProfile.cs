using AutoMapper;
using WindowsClient.Services.Storage.FileStorage.XmlModels.MaterialBalance;
using UIModels = WindowsClient.Shared.UIModels.MaterialBalance;
using EngineShared = SMBS.Shared;

namespace WindowsClient.Services.Storage.Transformations
{
    public class ProductionEstimationResultRowProfile : Profile
    {
        public ProductionEstimationResultRowProfile()
        {
            CreateMap<ProductionEstimationResultRow, UIModels.ProductionEstimationResultRow>()
                .ReverseMap();

            CreateMap<UIModels.ProductionEstimationResultRow, EngineShared.Result.ProductionEstimationResultRow>()
                .ReverseMap();
        }
    }
}
