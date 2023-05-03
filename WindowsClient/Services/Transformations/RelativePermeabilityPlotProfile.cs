using AutoMapper;
using UIModels = WindowsClient.Shared.UIModels.MaterialBalance;
using EngineShared = SMBS.Shared;

namespace WindowsClient.Services.Storage.Transformations
{
    public class RelativePermeabilityPlotProfile : Profile
    {
        public RelativePermeabilityPlotProfile()
        {
            CreateMap <EngineShared.Result.RelativePermeabilityPlot, UIModels.RelativePermeabilityPlot > ()
                  .ReverseMap();
        }
    }
}
