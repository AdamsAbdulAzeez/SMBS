using Prism.Commands;
using WindowsClient.ApplicationLayout.ToastNotification;
using WindowsClient.Services.Storage;
using WindowsClient.Shared.ErrorHandling;

namespace WindowsClient.Features.CreateModelWindow.Commands
{
    internal class UpdateModelCommand : DelegateCommandBase
    {
        private readonly IModelStore _modelStore;
        private readonly IToastNotification _toastNotification;

        public UpdateModelCommand(IModelStore modelStore,
            IToastNotification toastNotification,
            CreateModelViewModel viewModel)
        {
            _modelStore = modelStore;
            _toastNotification = toastNotification;
            ViewModel = viewModel;
        }

        //TODO: Add implementation to ask user for confirmation when fluid type changes in existing tank and want to invalidate certain properties for new selection
        protected override async void Execute(object parameter)
        {
            ViewModel.SetIsLoading(true);

            IActionResult result = await _modelStore.UpdateModelAsync(ViewModel.Model);
            
            ViewModel.SetIsLoading(false);

            if (result.IsFailure)
                _toastNotification.ShowError($"Unable to update model. {string.Join(',', result.Errors)}", "Update Model");

            if (result.IsSuccess)
                ViewModel.View.Close();
        }

        protected override bool CanExecute(object parameter) => true;
        public CreateModelViewModel ViewModel { get; }
    }

}
