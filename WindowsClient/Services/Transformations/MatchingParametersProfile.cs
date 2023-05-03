using AutoMapper;
using WindowsClient.Shared.UIModels.MaterialBalance;
using System;

namespace WindowsClient.Services.Storage.Transformations
{
    public class MatchingParametersProfile : Profile
    {
        public MatchingParametersProfile() =>
            CreateMap<PVTLibrary.PVTMatching.MatchingParameters, MatchingParameters>()
            .ConvertUsing(typeof(VariableConverter));
    }

    class VariableConverter : ITypeConverter<PVTLibrary.PVTMatching.MatchingParameters, MatchingParameters>
    {
        private const int NUMBER_OF_DP = 5;
        public MatchingParameters Convert(PVTLibrary.PVTMatching.MatchingParameters source, 
            MatchingParameters destination, ResolutionContext context)
        {
            if (source == null) return null;
            if (destination == null) destination = new MatchingParameters();

            destination.C1 = Math.Round(source.C1, NUMBER_OF_DP);
            destination.C2 = Math.Round(source.C2, NUMBER_OF_DP);
            destination.C3 = Math.Round(source.C3, NUMBER_OF_DP);
            destination.C4 = Math.Round(source.C4, NUMBER_OF_DP);
            destination.StdDeviation = Math.Round(source.StdDeviation, 15);

            destination.Name = source.Name;
            return destination;
        }
    }
}
