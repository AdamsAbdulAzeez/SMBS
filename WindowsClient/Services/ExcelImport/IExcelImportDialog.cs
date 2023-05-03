using NPOI.SS.UserModel;

namespace WindowsClient.Services.ExcelImport
{
    public interface IExcelImportDialog
    {
        bool? ShowAsDialog();
        void Close();
        IWorkbook Workbook { get; }
        string SelectedSheet { get; }
    }
}
