using NPOI.SS.UserModel;
using System;
using System.Windows;
using WindowsClient.ApplicationLayout.ToastNotification;

namespace WindowsClient.Services.ExcelImport
{
    /// <summary>
    /// Interaction logic for ExcelImportDialog.xaml
    /// </summary>
    public partial class ExcelImportDialog : Window, IExcelImportDialog
    {

        public ExcelImportDialog()
        {
            InitializeComponent();
            viewModel = new ExcelImportDialogViewModel(this,
                Ioc.Resolve<IToastNotification>());
            DataContext = viewModel;
        }

        private ExcelImportDialogViewModel viewModel;
        public string SelectedSheet => viewModel.SelectedSheet;
        public IWorkbook Workbook => viewModel.Workbook;

        bool? IExcelImportDialog.ShowAsDialog()
        {
            Owner = Application.Current.MainWindow;
            ShowDialog();
            if (!viewModel.IsDoneClicked) viewModel.Workbook?.Close();
            return viewModel.IsDoneClicked;
        }
    }
}
