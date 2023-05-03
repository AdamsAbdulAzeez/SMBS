using System;
using Microsoft.Win32;
using Prism.Commands;
using WindowsClient.ApplicationLayout.ToastNotification;
using WindowsClient.Services.Storage;

namespace WindowsClient.ApplicationLayout.RibbonTabs.ModellingTab.Commands
{
    internal class ImportCommand : DelegateCommandBase
    {
        private readonly IToastNotification _toastNotification;
        private readonly IModelStore _modelStore;

        public ImportCommand(IModelStore modelStore, IToastNotification toastNotification)
        {
            _toastNotification = toastNotification;
            _modelStore = modelStore;
        }

        protected override bool CanExecute(object parameter) => true;

        protected override async void Execute(object parameter)
        {
            var openFileDialog = new OpenFileDialog();
            try
            {
                if (openFileDialog.ShowDialog() != true) return;

                if (string.IsNullOrEmpty(openFileDialog.FileName)) throw new Exception();

                var result = await _modelStore.AddModelFromPath(openFileDialog.FileName);
                if (result.IsFailure) throw new Exception();
                _toastNotification.ShowInformation("File imported", "Completed!");
            }
            catch
            {
                _toastNotification.ShowError("Failed to import file", "Operation failed!");
            }
        }
    }
}
