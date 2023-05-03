using System.Threading.Tasks;
using WindowsClient.Shared.UIModels.MaterialBalance;

namespace WindowsClient.Services.Calculation.FractionalFlowMatching
{
    interface IFractionalFlowMatchingService
    {
        Task<FractionalMatchResult> MatchAsync(Tank tank, FracMatchingChoice matchingChoice = FracMatchingChoice.Fw);
    }
}
