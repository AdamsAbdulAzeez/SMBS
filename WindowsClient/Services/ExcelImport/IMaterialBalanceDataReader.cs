using System.Collections.Generic;
using System.Threading.Tasks;
using WindowsClient.Shared.ErrorHandling;
using WindowsClient.Shared.UIModels.MaterialBalance;

namespace WindowsClient.Services.ExcelImport
{
    interface IMaterialBalanceDataReader
    {
        Task<IActionResult<IList<ProductionDataRow>>> ReadProductionData();
        Task<IActionResult<IList<PvtDataRow>>> ReadExternalPvtData(FluidType fluidType);
        Task<IActionResult<Tank>> ReadHistoryMatchingInputData();
        Task<IActionResult<Tank>> ReadPvtMatchingInputData(FluidType fluidType);
    }
}
