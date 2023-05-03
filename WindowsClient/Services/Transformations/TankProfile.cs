using AutoMapper;
using CypherCrescent.Units.Services;
using WindowsClient.Services.Storage.FileStorage.XmlModels.MaterialBalance;
using UIModels = WindowsClient.Shared.UIModels.MaterialBalance;
using EngineShared = SMBS.Shared;
using WindowsClient.Services.Transformations.Extensions;

namespace WindowsClient.Services.Storage.Transformations
{
    public class TankProfile : Profile
    {
        public TankProfile()
        {
            CreateMap<Tank, UIModels.Tank>()
                .ForMember(uiTank => uiTank.ModelId, opt => opt.MapFrom(dbTank => dbTank.MaterialBalanceModelId))
                .AfterMap(HookUpUnits)
                .ReverseMap();

            CreateMap<UIModels.Tank, EngineShared.Tank>()
                .ForMember(
                    engineTank => engineTank.RelativePermeabilityPlot,
                    opt => opt.SetMappingOrder(0))
                .ForMember(
                    engineTank => engineTank.RelPermData,
                    opts =>
                    {
                        opts.SetMappingOrder(1);
                    })
                .Ignore(member => member.AreaDepthData)
                .Ignore(member => member.PoreVolumeDepth)
                .ReverseMap();
        }

        private void HookUpUnits(Tank _, UIModels.Tank tank, ResolutionContext context)
        {
            var unitService = context.Options.ServiceCtor(typeof(IUnitService)) as IUnitService;
            unitService.RegisterVariable(tank.InitialReservoirPressure);
            unitService.RegisterVariable(tank.Rock.Porosity.CurrentValue);
            unitService.RegisterVariable(tank.ConnateWaterSaturation);
        }
    }
}
