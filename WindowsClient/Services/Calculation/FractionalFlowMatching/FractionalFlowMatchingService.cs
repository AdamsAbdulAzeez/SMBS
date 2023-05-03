using AutoMapper;
using SMBS.Engine;
using System.Threading.Tasks;
using WindowsClient.Shared.UIModels.MaterialBalance;
using EngineShared = SMBS.Shared;

namespace WindowsClient.Services.Calculation.FractionalFlowMatching
{
    public class FractionalFlowMatchingService : IFractionalFlowMatchingService
    {
        private readonly IMapper _mapper;
        public FractionalFlowMatchingService(IMapper mapper) => _mapper = mapper;

        public async Task<FractionalMatchResult> MatchAsync(Tank tank, FracMatchingChoice matchingChoice = FracMatchingChoice.Fw)
        {
            var engineTank = _mapper.Map<EngineShared.Tank>(tank);


            SetUp setUp = new SetUp(engineTank);
            await Task.Run(() => setUp.FracMatch(engineTank, (SetUp.FracMatchingChoice)matchingChoice));

            var fractionalFlowResult = _mapper.Map<FractionalMatchResult>(engineTank.FractionalMatchResult);

            return fractionalFlowResult;
        }
    }
}
