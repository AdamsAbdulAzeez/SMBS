using AutoMapper;
using FileStorageModels = WindowsClient.Services.Storage.FileStorage.XmlModels.MaterialBalance;
using UIModels = WindowsClient.Shared.UIModels.MaterialBalance;
using SMBS.Shared.DataImport;

namespace WindowsClient.Services.Storage.Transformations
{
    public class PoreVolumeDepthRowProfile : Profile
    {
        public PoreVolumeDepthRowProfile()
        {
            CreateMap<FileStorageModels.PoreVolumeDepthRow, UIModels.PoreVolumeDepthRow>()
                .ReverseMap();

            CreateMap<FileStorageModels.PoreVolumeDepthRow,  PoreVolumeDepthRow>()
               .ReverseMap();
        }
    }
}
