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

namespace WindowsClient.Features.Transmissibility
{
    /// <summary>
    /// Interaction logic for TransmissibilityWindow.xaml
    /// </summary>
    public partial class TransmissibilityWindow : Window, ITransmissibilityWindow
    {
        public TransmissibilityWindow()
        {
            InitializeComponent();
            DataContext = new TransmissibilityViewModel(this);
        }
        public void CloseDialog() => Close();       
        public void OpenDialog() => ShowDialog();        
    }
}
