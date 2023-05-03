using System.Windows;

namespace WindowsClient.Features.Transmissibility
{
    /// <summary>
    /// Interaction logic for Transmissibility.xaml
    /// </summary>
    public partial class Transmissibility : Window, ITransmissibilityWindow
    {
        public Transmissibility()
        {
            InitializeComponent();
            DataContext = new TransmissibilityViewModel(this);
        }

        public void CloseDialog()
        {
        }

        public void OpenDialog() => OpenDialog();
        
    }
}
