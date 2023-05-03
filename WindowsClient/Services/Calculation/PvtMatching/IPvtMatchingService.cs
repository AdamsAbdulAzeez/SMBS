using System.Threading.Tasks;
using WindowsClient.Shared.UIModels.MaterialBalance;

namespace WindowsClient.Services.Calculation.PvtMatching
{
    internal interface IPvtMatchingService
    {
        Task<PvtMatchingResult> MatchAsync(Tank tank);
    }
}