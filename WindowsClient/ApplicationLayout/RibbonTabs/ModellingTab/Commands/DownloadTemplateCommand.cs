using System.IO;
using Microsoft.Win32;
using Prism.Commands;
using WindowsClient.ApplicationLayout.ToastNotification;

namespace WindowsClient.ApplicationLayout.RibbonTabs.ModellingTab.Commands
{
    internal class DownloadTemplateCommand : DelegateCommandBase
    {
        private readonly IToastNotification _toastNotification;

        public DownloadTemplateCommand(IToastNotification toastNotification) => 
            _toastNotification = toastNotification;

        protected override bool CanExecute(object parameter) => true;

        protected override void Execute(object parameter)
        {
            var saveFileDialog = new SaveFileDialog
            {
                FileName = "SMBS Template.xlsx",
                Filter = "Excel File (xlsx) | *.xlsx",
            };
            try
            {
                if (saveFileDialog.ShowDialog() != true) return;

                if (File.Exists(saveFileDialog.FileName)) File.Delete(saveFileDialog.FileName);

                File.Copy(GetTemplatePath(), saveFileDialog.FileName);
            }
            catch
            {
                _toastNotification.ShowError("Failed to download template", "Operation failed!");
            }


        }

        private static string GetTemplatePath() =>
            "../../../Services/ExcelImport/Templates/SMBSTemplate.xlsx";
    }
}
