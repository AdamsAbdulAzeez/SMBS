using Prism.Events;
using Prism.Ioc;

namespace WindowsClient.ApplicationLayout.StatusBar
{
    /// <summary>
    /// Interaction logic for StatusBarView.xaml
    /// </summary>
    public partial class StatusBarView
    {
        public StatusBarView()
        {
            InitializeComponent();
            DataContext = new StatusBarViewModel(Ioc.Resolve<IEventAggregator>());
        }
    }
}
