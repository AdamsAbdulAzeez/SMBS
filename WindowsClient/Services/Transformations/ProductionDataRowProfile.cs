using AutoMapper;
using WindowsClient.Services.Storage.FileStorage.XmlModels.MaterialBalance;
using UIModels = WindowsClient.Shared.UIModels.MaterialBalance;
using EngineShared = SMBS.Shared;

namespace WindowsClient.Services.Storage.Transformations
{
    public class ProductionDataRowProfile : Profile
    {
        public ProductionDataRowProfile()
        {
            CreateMap<ProductionDataRow, UIModels.ProductionDataRow>()
                .ReverseMap();

            CreateMap<UIModels.ProductionDataRow, EngineShared.DataImport.ProductionDataRow>()
                .ReverseMap();
        }
    }
}
