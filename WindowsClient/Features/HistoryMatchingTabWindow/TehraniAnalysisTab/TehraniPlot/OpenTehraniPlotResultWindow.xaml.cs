using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WindowsClient.Features.HistoryMatchingTabWindow.TehraniAnalysisTab.TehraniPlot
{
    /// <summary>
    /// Interaction logic for OpenTehraniPlotResultWindow.xaml
    /// </summary>
    public partial class OpenTehraniPlotResultWindow : Window, IOpenTehraniPlotResultWindow
    {
        public OpenTehraniPlotResultWindow()
        {
            InitializeComponent();
        }

        void IOpenTehraniPlotResultWindow.ShowDialog(OpenTehraniPlotResultWindowViewModel viewModel)
        {
            Owner = Application.Current.MainWindow;
            DataContext = viewModel;
            base.Show();
        }

        private void OnCloseSetupWindow(object sender, RoutedEventArgs e) => Close();
    }
}
