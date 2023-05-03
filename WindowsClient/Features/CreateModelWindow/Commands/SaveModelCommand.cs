using Prism.Commands;
using WindowsClient.ApplicationLayout.ToastNotification;
using WindowsClient.Services.Storage;
using WindowsClient.Shared.ErrorHandling;

namespace WindowsClient.Features.CreateModelWindow.Commands
{
    internal class SaveModelCommand : DelegateCommandBase
    {
        private readonly IModelStore _modelStore;
        private readonly IToastNotification _toastNotification;

        public SaveModelCommand(IModelStore modelStore,
            IToastNotification toastNotification,
            CreateModelViewModel viewModel)
        {
            _modelStore = modelStore;
            _toastNotification = toastNotification;
            ViewModel = viewModel;
        }

        protected override async void Execute(object parameter)
        {
            var model = ViewModel.Model;
            ViewModel.SetIsLoading(true);

            IActionResult result = await _modelStore.SaveModelAsync(model);
            ViewModel.SetIsLoading(false);
            if (result.IsFailure)
                _toastNotification.ShowError($"Unable to save model. {string.Join(',', result.Errors)}",
                    "Create Model");

            if (result.IsSuccess)
                ViewModel.View.Close();
        }

        protected override bool CanExecute(object parameter)
        {
            return ViewModel.Model.Tanks.Count > 0 &&
                   !string.IsNullOrEmpty(ViewModel.Model.Name);
        }

        protected CreateModelViewModel ViewModel { get; set; }
    }

}
