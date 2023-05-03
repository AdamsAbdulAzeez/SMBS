using AutoMapper;
using FileStorageModels = WindowsClient.Services.Storage.FileStorage.XmlModels.MaterialBalance;
using UIModels = WindowsClient.Shared.UIModels.MaterialBalance;

namespace WindowsClient.Services.Storage.Transformations
{
    internal class PvtInitialConditionProfile : Profile
    {
        public PvtInitialConditionProfile() => 
            CreateMap<FileStorageModels.PvtInitialCondition, UIModels.PvtInitialCondition>()
            .ReverseMap();
    }
}
