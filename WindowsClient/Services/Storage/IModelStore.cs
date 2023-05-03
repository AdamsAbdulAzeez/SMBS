using System;
using System.Threading.Tasks;
using WindowsClient.Shared.ErrorHandling;
using WindowsClient.Shared.UIModels.MaterialBalance;

namespace WindowsClient.Services.Storage
{
    internal interface IModelStore
    {
        Task LoadSavedFilesAsync();
        Task<IActionResult> SaveModelAsync(Model model);
        Task<IActionResult> UpdateModelAsync(Model model);
        Task<IActionResult> DeleteModelAsync(Model model);
        Task<IActionResult> UpdateTankAsync(Tank tank);
        Task<IActionResult<Model>> GetModelAsync(Guid modelId);
        Task<IActionResult> AddModelFromPath(string modelPath);
        event Action<bool> OnBusyChanged;
        event Action<Model> ModelAdded;
        event Action<Model> ModelUpdated;
        event Action<Model> ModelDeleted;
    }
}
