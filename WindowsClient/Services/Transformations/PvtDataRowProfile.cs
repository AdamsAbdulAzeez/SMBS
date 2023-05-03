using AutoMapper;
using FileStorageModels = WindowsClient.Services.Storage.FileStorage.XmlModels.MaterialBalance;
using UIModels = WindowsClient.Shared.UIModels.MaterialBalance;
using PVTLibrary;

namespace WindowsClient.Services.Storage.Transformations
{
    public class PvtDataRowProfile : Profile
    {
        public PvtDataRowProfile()
        {
            CreateMap<FileStorageModels.PvtDataRow, UIModels.PvtDataRow>()
                .ReverseMap();

            CreateMap<UIModels.PvtDataRow, PvtDataRow>()
               .ReverseMap();
        }
    }
}
