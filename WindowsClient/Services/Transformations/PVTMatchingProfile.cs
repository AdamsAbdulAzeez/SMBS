using AutoMapper;
using PVTLibrary;

namespace WindowsClient.Services.Storage.Transformations
{
    public class PVTMatchingProfile : Profile
    {
        public PVTMatchingProfile()
        {
            CreateMap<FileStorage.XmlModels.MaterialBalance.PVTMatching, PVTMatching>()
                //.ForMember(libPvtMatching => libPvtMatching.ChoosenModels, cd => cd.MapFrom(uiPvtMatching => uiPvtMatching.ChoosenModels))
                .ReverseMap();
        }
    }
}
