using NPOI.SS.UserModel;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.IO;
using WindowsClient.ApplicationLayout.ToastNotification;
using WindowsClient.Services.ExcelImport.Commands;

namespace WindowsClient.Services.ExcelImport
{
    internal class ExcelImportDialogViewModel : BindableBase
    {
        public ExcelImportDialogViewModel(IExcelImportDialog excelImportDialog,
            IToastNotification toastNotification)
        {
            DoneCommand = new DoneCommand(toastNotification);
            BrowseFileCommand = new BrowseFileCommand();
            View = excelImportDialog;
            WorkbookChanged += OnWorkbookChanged;
            FileNameChanged += OnFileNameChanged;
        }

        private void OnWorkbookChanged(IWorkbook workbook)
        {
            Workbook = workbook;
            SheetNames.Clear();

            for (int i = 0; i < Workbook.NumberOfSheets; i++)
            {
                SheetNames.Add(Workbook.GetSheetName(i));
            }
            RaisePropertyChanged(nameof(CanSelectSheet));
        }

        private void OnFileNameChanged(string fileFullPath)
        {
            FileName = Path.GetFileName(fileFullPath);
            RaisePropertyChanged(nameof(FileName));
        }

        public IExcelImportDialog View { get; }

        private string fileName;
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        private string selectedSheet;
        public string SelectedSheet
        {
            get { return selectedSheet; }
            set
            {
                selectedSheet = value;
                DoneCommand.RaiseCanExecuteChanged();
            }
        }

        public bool CanSelectSheet => SheetNames.Count > 0;
        public ObservableCollection<string> SheetNames { get; set; } = new();
        public DoneCommand DoneCommand { get; }
        public BrowseFileCommand BrowseFileCommand { get; }
        internal IWorkbook Workbook { get; set; }
        internal Action<IWorkbook> WorkbookChanged;
        internal Action<string> FileNameChanged;
        internal bool IsDoneClicked { get; set; }
    }
}
