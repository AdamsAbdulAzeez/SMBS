using Prism.Events;
using AutoMapper;
using WindowsClient.Features.CreateModel;
using WindowsClient.Features.Transmissibility;
using WindowsClient.Services.Storage;
using WindowsClient.Shared.UserInteractions;
using WindowsClient.ApplicationLayout.ToastNotification;
using System.Windows.Controls;
using WindowsClient.Shared.UIModels.MaterialBalance;
using WindowsClient.ApplicationLayout.LeftPane.DataExplorer.Events;

namespace WindowsClient.ApplicationLayout.LeftPane.DataExplorer
{
    /// <summary>
    /// Interaction logic for DataExplorerView.xaml
    /// </summary>
    public partial class DataExplorerView 
    {
        private IEventAggregator _eventAggregator;
        public DataExplorerView()
        {
            _eventAggregator = Ioc.Resolve<IEventAggregator>();
            InitializeComponent();
            DataContext = new DataExplorerViewModel(
                Ioc.Resolve<IModelStore>(),
                _eventAggregator,
                Ioc.Resolve<ITransmissibilityWindow>(),
                Ioc.Resolve<ICreateModelView>,
                Ioc.Resolve<IConfirmActionDialog>,
                Ioc.Resolve<IMapper>(),
                Ioc.Resolve<IToastNotification>());
        }

        private void ContextMenuOpened(object sender, System.Windows.RoutedEventArgs e)
        {
            (DataContext as DataExplorerViewModel)?.OpenTransmissibilityWindowCommand
                .RaiseCanExecuteChanged();
        }

        private void OnModelSelected(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var model = (sender as TextBlock).DataContext as Model;
            _eventAggregator.GetEvent<ModelSelectedEvent>().Publish(model.Id);
        }
    }
}
