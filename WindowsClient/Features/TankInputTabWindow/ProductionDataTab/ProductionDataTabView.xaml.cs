using System.Windows.Controls;
using System.Windows;
using WindowsClient.Shared.UIModels.MaterialBalance;

namespace WindowsClient.Features.TankInputTabWindow.ProductionDataTab
{
    /// <summary>
    /// Interaction logic for ProductionDataTabView.xaml
    /// </summary>
    public partial class ProductionDataTabView : UserControl
    {
        public ProductionDataTabView()
        {
            InitializeComponent();
            Loaded += ProductionDataTabView_Loaded;
        }

        private void ProductionDataTabView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var vm = DataContext as ProductionDataTabViewModel;
            if (vm.Tank.FlowingFluid == FluidType.Gas) 
                oilProducedColumn.Visibility = Visibility.Collapsed;
            else oilProducedColumn.Visibility = Visibility.Visible;
        }
    }
}
