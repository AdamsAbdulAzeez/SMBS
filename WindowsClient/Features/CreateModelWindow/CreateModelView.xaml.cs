using System.Windows;
using System.Windows.Controls;
using WindowsClient.ApplicationLayout.ToastNotification;
using WindowsClient.Features.CreateModel;
using WindowsClient.Services.Storage;
using WindowsClient.Shared.Features;
using WindowsClient.Shared.UIModels.MaterialBalance;
using WindowsClient.Shared.UserInteractions;

namespace WindowsClient.Features.CreateModelWindow
{
    /// <summary>
    /// Interaction logic for CreateModelView.xaml
    /// </summary>
    public partial class CreateModelWindowView : ICreateModelView
    {
        public CreateModelWindowView()
        {
            InitializeComponent();
            DataContext = new CreateModelViewModel(this, 
                Ioc.Resolve<IModelStore>(), 
                Ioc.Resolve<IToastNotification>(),
                Ioc.Resolve<IConfirmActionDialog>);
        }

        private void OnCellSelectedBeginEdit(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource.GetType() == typeof(DataGridCell))
            {
                DataGrid grd = (DataGrid)sender;
                grd.BeginEdit(e);
            }
        }

        public void ShowViewAsCreateDialog()
        {
            Owner = Application.Current.MainWindow;
            ShowDialog();
        }

        void ICreateModelView.ShowViewAsEditDialog(Model model)
        {
            var dataContext = DataContext as CreateModelViewModel;
            dataContext.DisplayMode = DisplayMode.Edit;
            dataContext.Model = model;
            Owner = Application.Current.MainWindow;
            ShowDialog();
        }
    }
}
