using System;
using ActiproSoftware.Windows.Controls.Docking;
using AutoMapper;
using Prism.Events;
using WindowsClient.ApplicationLayout.ToastNotification;
using WindowsClient.Services.ExcelImport;
using WindowsClient.Services.Storage;
using WindowsClient.Shared.UIModels.MaterialBalance;
using WindowsClient.Shared.UserInteractions;
using WindowsClient.Shared.Visualisation;
using WindowsClient.Shared.Visualisation.CartesianChart;

namespace WindowsClient.Features.TankInputTabWindow
{
    public partial class TankInputTabWindowView : DocumentWindow
    {
        internal TankInputTabWindowView(Tank tank,
            IModelStore modelStore,
            IEventAggregator eventAggregator,
            IToastNotification toastNotification, 
            IMaterialBalanceDataReader materialBalanceDataReader,
            IMapper mapper,
            Func<IConfirmActionDialog> confirmActionDialog,
            Func<ICartesianChartControl> getCartesianChartControl,
            Func<IChartWrapper> getChartControl)
        {
            InitializeComponent();
            Title = tank.Name + " Tank Input";
            DataContext = new TankInputTabWindowViewModel(
                tank,
                modelStore,
                eventAggregator,
                toastNotification,
                materialBalanceDataReader,
                mapper,
                confirmActionDialog,
                getCartesianChartControl,
                getChartControl);
        }
    }
}
