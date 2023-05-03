using Prism.Events;

namespace WindowsClient.ApplicationLayout.RibbonTabs.AnalyticsTab
{
    /// <summary>
    /// Interaction logic for ModellingTabView.xaml
    /// </summary>
    public partial class AnalyticsTabView
    {
        public AnalyticsTabView()
        {
            InitializeComponent();
            DataContext = new AnalyticsTabViewModel(Ioc.Resolve<IEventAggregator>());
        }
    }
}
