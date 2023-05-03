using ActiproSoftware.Windows.Controls.Docking;
using AutoMapper;
using Prism.Events;
using System;
using WindowsClient.ApplicationLayout.ToastNotification;
using WindowsClient.Features.HistoryMatchingTabWindow.TehraniAnalysisTab.RegressionSetup;
using WindowsClient.Features.HistoryMatchingTabWindow.TehraniAnalysisTab.TehraniPlot;
using WindowsClient.Services.Calculation;
using WindowsClient.Services.ExcelImport;
using WindowsClient.Services.Storage;
using WindowsClient.Shared.UIModels.MaterialBalance;
using WindowsClient.Shared.UserInteractions;
using WindowsClient.Shared.Visualisation;
using WindowsClient.Shared.Visualisation.CartesianChart;

namespace WindowsClient.Features.ProductionPredictionTabWindow
{
    /// <summary>
    /// Interaction logic for ProductionPredictionTabView.xaml
    /// </summary>
    public partial class ProductionPredictionTabView : DocumentWindow
    {
        internal ProductionPredictionTabView(Tank tank,
            IModelStore modelStore,
            IEventAggregator eventAggregator,
            IToastNotification toastNotification,
            IMaterialBalanceDataReader materialBalanceDataReader,
            Func<ICartesianChartControl> getCartesianChartControl,
            Func<IChartWrapper> getTehraniChartControl,
            Func<IRegressionSetupWindow> getTehraniSetupWindow,
            Func<IOpenTehraniPlotResultWindow> getTehraniOpenResultWindow,
            IMapper mapper,
            ICalculationServices calculator,
            Func<IConfirmActionDialog> confirmActionDialog)
        {
            InitializeComponent();
            Title = tank.Name + " Production Prediction";
            DataContext = new ProductionPredictionViewModel();
        }
       
       
    }
}
