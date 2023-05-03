using Prism.Commands;
using WindowsClient.ApplicationLayout.ToastNotification;

namespace WindowsClient.Services.ExcelImport.Commands
{
    internal sealed class DoneCommand : DelegateCommandBase
    {
        private readonly IToastNotification _toastNotification;

        public DoneCommand(IToastNotification toastNotification) => _toastNotification = toastNotification;

        protected override bool CanExecute(object parameter)
        {
            var viewModel = parameter as ExcelImportDialogViewModel;
            return !string.IsNullOrEmpty(viewModel?.SelectedSheet);
        }

        protected override void Execute(object parameter)
        {
            var viewModel = parameter as ExcelImportDialogViewModel;
            viewModel.IsDoneClicked = true;
            viewModel.View.Close();
        }
    }

}
