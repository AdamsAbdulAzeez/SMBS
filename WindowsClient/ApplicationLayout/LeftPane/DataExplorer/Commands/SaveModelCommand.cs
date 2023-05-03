using Prism.Commands;
using WindowsClient.ApplicationLayout.ToastNotification;
using WindowsClient.Services.Storage;
using WindowsClient.Shared.ErrorHandling;
using WindowsClient.Shared.UIModels.MaterialBalance;

namespace WindowsClient.ApplicationLayout.LeftPane.DataExplorer.Commands
{
    internal class SaveModelCommand : DelegateCommandBase
    {
        private readonly IModelStore _modelStore;
        private readonly IToastNotification _toastNotification;

        public SaveModelCommand(IModelStore modelStore,
            IToastNotification toastNotification)
        {
            _modelStore = modelStore;
            _toastNotification = toastNotification;
        }

        protected override async void Execute(object parameter)
        {
            if (parameter is not Model model) return;

            IActionResult result = await _modelStore.UpdateModelAsync(model);

            if (result.IsSuccess)
            {
                _toastNotification.ShowInformation("Model has been saved!", "Operation Successful");
                return;
            }

            if (result.IsFailure)
                _toastNotification.ShowError($"Unable to save model. {string.Join(',', result.Errors)}",
                    "Operation failed");
        }

        protected override bool CanExecute(object parameter) => true;
    }
}
