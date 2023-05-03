using Microsoft.Win32;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Prism.Commands;
using System.Collections.Generic;
using System.IO;
using WindowsClient.Shared.ErrorHandling;

namespace WindowsClient.Services.ExcelImport.Commands
{
    internal sealed class BrowseFileCommand : DelegateCommandBase
    {
        protected override bool CanExecute(object parameter)
        {
            return true;
        }

        protected override void Execute(object parameter)
        {
            if (parameter is not ExcelImportDialogViewModel viewModel) return;

            var fileDialog = new OpenFileDialog
            {
                Filter = "Excel Files (xls, xlsx) | *.xlsx;*.xls;*.xlsm"
            };

            var result = fileDialog.ShowDialog();

            if (!result.Value) return;

            var readResult = ReadWorkbook(fileDialog.FileName);
            if (readResult.IsFailure) return;
            viewModel.FileNameChanged?.Invoke(fileDialog.FileName);
            viewModel.WorkbookChanged?.Invoke(readResult.Payload);
        }

        private IActionResult<IWorkbook> ReadWorkbook(string filePath)
        {
            IWorkbook book;
            try
            {
                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                try
                {
                    book = new XSSFWorkbook(fs);
                }
                catch
                {
                    book = null;
                }

                if (book == null)
                {
                    book = new HSSFWorkbook(fs);
                }

                return new SuccessfulAction<IWorkbook>(book);
            }
            catch
            {
                return new FailedResult<IWorkbook>(new List<string> { "Failed to read workbook" });
            }
        }
    }
}
