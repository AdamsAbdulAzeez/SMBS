using System.Windows;

namespace WindowsClient.Shared.Visualisation
{
    /// <summary>
    /// Interaction logic for PageHeader.xaml
    /// </summary>
    public partial class PageHeader
    {
        public PageHeader(string pageName)
        {
            InitializeComponent();
            Text = pageName;
            FontSize = 14;
            Margin = new Thickness(3);
        }
    }
}
