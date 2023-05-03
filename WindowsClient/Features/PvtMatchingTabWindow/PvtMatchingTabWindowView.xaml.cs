using Prism.Events;
using System;
using WindowsClient.ApplicationLayout.ToastNotification;
using WindowsClient.Services.Calculation;
using WindowsClient.Services.ExcelImport;
using WindowsClient.Services.Storage;
using WindowsClient.Shared.UIModels.MaterialBalance;
using WindowsClient.Shared.Visualisation.CartesianChart;

namespace WindowsClient.Features.PvtMatchingTabWindow
{
    public partial class PvtMatchingTabWindowView
    {
        internal PvtMatchingTabWindowView(Tank tank,
            IModelStore modelStore,
            IEventAggregator eventAggregator,
            IToastNotification toastNotification,
            IMaterialBalanceDataReader materialBalanceDataReader,
            ICalculationServices calculationServices,
            Func<ICartesianChartControl> chartControl)
        {
            InitializeComponent();
            Title = tank.Name + " PVT ";
            DataContext = new PvtMatchingTabWindowViewModel(tank, 
                modelStore, 
                eventAggregator, 
                toastNotification, 
                materialBalanceDataReader, 
                calculationServices, 
                chartControl);
        }
    }
}
