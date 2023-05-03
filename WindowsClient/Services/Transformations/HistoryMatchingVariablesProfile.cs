﻿using AutoMapper;
using WindowsClient.Services.Storage.FileStorage.XmlModels.MaterialBalance;
using UIModels = WindowsClient.Shared.UIModels.MaterialBalance;

namespace WindowsClient.Services.Storage.Transformations
{
    public class HistoryMatchingVariablesProfile : Profile
    {
        public HistoryMatchingVariablesProfile()
        {
            CreateMap<HistoryMatchingVariables, UIModels.HistoryMatchingVariables>()
                .ReverseMap();
        }
    }
}
