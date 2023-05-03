using Prism.Events;
using WindowsClient.ApplicationLayout.ToastNotification;
using WindowsClient.Features.CreateModel;
using WindowsClient.Services.Storage;

namespace WindowsClient.ApplicationLayout.RibbonTabs.ModellingTab
{
    /// <summary>
    /// Interaction logic for ModellingTabView.xaml
    /// </summary>
    public partial class ModellingTabView
    {
        public ModellingTabView()
        {
            InitializeComponent();
            DataContext = new ModellingTabViewModel(Ioc.Resolve<ICreateModelView>,
                Ioc.Resolve<IEventAggregator>(),
                Ioc.Resolve<IToastNotification>(),
                Ioc.Resolve<IModelStore>());
        }
    }
}
