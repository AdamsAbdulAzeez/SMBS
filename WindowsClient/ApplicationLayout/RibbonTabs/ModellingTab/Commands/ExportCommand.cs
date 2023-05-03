using System.IO;
using Microsoft.Win32;
using Prism.Commands;
using WindowsClient.ApplicationLayout.ToastNotification;

namespace WindowsClient.ApplicationLayout.RibbonTabs.ModellingTab.Commands
{
    internal class ExportCommand : DelegateCommandBase
    {
        private readonly IToastNotification _toastNotification;

        public ExportCommand(IToastNotification toastNotification) => _toastNotification = toastNotification;

        protected override bool CanExecute(object parameter) => true;

        protected override void Execute(object parameter)
        {
            if (parameter is not ModellingTabViewModel viewModel || viewModel.SelectedModel == null)
            {
                _toastNotification.ShowError("Select a model to export", "Operation Failed");
                return;
            }
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "SMBS File | *.xml",
            };
            saveFileDialog.FileName = viewModel.SelectedModel.Name;
            try
            {
                if (saveFileDialog.ShowDialog() != true) return;

                if (File.Exists(saveFileDialog.FileName)) File.Delete(saveFileDialog.FileName);

                File.Copy(Path.Combine(filePath, viewModel.SelectedModel.Name+".xml"), saveFileDialog.FileName);

                _toastNotification.ShowInformation("File exported", "Completed!");

            }
            catch
            {
                _toastNotification.ShowError("Failed to export file", "Operation failed!");
            }
        }

        private const string filePath ="C:/ProgramData/SMBS/Dev/";
    }
}
