using AutoMapper;
using WindowsClient.Services.Storage.FileStorage.XmlModels.MaterialBalance;
using WindowsClient.Shared.UIModels.MaterialBalance;

namespace WindowsClient.Services.Storage.Transformations
{
    public class MaterialBalanceModelProfile : Profile
    {
        public MaterialBalanceModelProfile()
        {
            CreateMap<MaterialBalanceModel, Model>()
            .ForMember(dbModel => dbModel.Tanks, opt => opt.MapFrom(fsTank => fsTank.Tanks)).ReverseMap();

            CreateMap<Model, Model>();
        }
    }
}
