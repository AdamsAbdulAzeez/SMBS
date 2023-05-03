using System.Windows;
using System.Windows.Controls;

namespace WindowsClient.Features.DashboardTabWindow.CartesianPlots.ConfigureAxes
{
    /// <summary>
    /// Interaction logic for ConfigureAxesWindow.xaml
    /// </summary>
    public partial class ConfigureAxesWindow: IConfigureAxesView
    {
        public ConfigureAxesWindow()
        {
            InitializeComponent();
        }

        private void OnCellSelectedBeginEdit(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource.GetType() == typeof(DataGridCell))
            {
                // Starts the Edit on the row;
                DataGrid grd = (DataGrid)sender;
                grd.BeginEdit(e);
            }
        }

        public void ShowViewAsDialog()
        {
            Owner = Application.Current.MainWindow;
            ShowDialog();
        }
    }
}
