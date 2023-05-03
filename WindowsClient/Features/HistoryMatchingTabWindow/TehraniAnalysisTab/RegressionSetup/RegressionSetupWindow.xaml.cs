using System.Windows;

namespace WindowsClient.Features.HistoryMatchingTabWindow.TehraniAnalysisTab.RegressionSetup
{
    /// <summary>
    /// Interaction logic for RegressionSetupView.xaml
    /// </summary>
    public partial class RegressionSetupWindow : Window, IRegressionSetupWindow
    {
        public RegressionSetupWindow()
        {
            InitializeComponent();
        }

        void IRegressionSetupWindow.ShowDialog(RegressionSetupWindowViewModel viewModel)
        {
            Owner = Application.Current.MainWindow;
            DataContext = viewModel;
            base.Show();
        }

        private void OnCloseSetupWindow(object sender, RoutedEventArgs e) => Close();
    }
}
