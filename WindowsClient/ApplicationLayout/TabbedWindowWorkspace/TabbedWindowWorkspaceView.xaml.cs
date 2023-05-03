using ActiproSoftware.Windows.Controls.Docking;
using Prism.Events;
using System;
using System.Linq;
using AutoMapper;
using WindowsClient.ApplicationLayout.ToastNotification;
using WindowsClient.Features;
using WindowsClient.Features.DashboardTabWindow;
using WindowsClient.Features.HistoryMatchingTabWindow;
using WindowsClient.Features.HistoryMatchingTabWindow.TehraniAnalysisTab.RegressionSetup;
using WindowsClient.Features.PvtMatchingTabWindow;
using WindowsClient.Features.PythonScripting.SciptingTabWindow;
using WindowsClient.Services.Calculation;
using WindowsClient.Services.ExcelImport;
using WindowsClient.Services.Storage;
using WindowsClient.Shared.UIModels.MaterialBalance;
using WindowsClient.Shared.Visualisation.CartesianChart;
using WindowsClient.Shared.UserInteractions;
using WindowsClient.Features.TankInputTabWindow;
using WindowsClient.Features.HistoryMatchingTabWindow.TehraniAnalysisTab.TehraniPlot;
using WindowsClient.Shared.Visualisation;
using WindowsClient.Features.ProductionPredictionTabWindow;

namespace WindowsClient.ApplicationLayout.TabbedWindowWorkspace
{
    public partial class TabbedWindowWorkspaceView : ITabWindowHost
    {
        public TabbedWindowWorkspaceView()
        {
            InitializeComponent();
            DataContext = new TabbedWindowWorkspaceViewModel(_eventAggregator, this);
        }

        bool ActivateWindowIfAlreadyOpen<TWindow>(Guid modelId) where TWindow : DocumentWindow
        {
            var existingWindow = tabControl.Windows
                .FirstOrDefault(window => window is TWindow && Guid.Parse(window.Tag?.ToString()) == modelId);

            if (existingWindow is null) return false;

            existingWindow.Activate();
            return true;
        }

        void ITabWindowHost.OpenHistoryMatchingTab(Tank tank)
        {
            if (ActivateWindowIfAlreadyOpen<HistoryMatchingTabWindowView>(tank.Id)) return;

            var window = new HistoryMatchingTabWindowView(
                tank,
                _modelStore,
                _eventAggregator,
                _toastNotification,
                _materialBalanceDataReader,
                Ioc.Resolve<ICartesianChartControl>,
                Ioc.Resolve<IChartWrapper>,
                Ioc.Resolve<IRegressionSetupWindow>,
                Ioc.Resolve<IOpenTehraniPlotResultWindow>,
                Ioc.Resolve<IMapper>(),
                _calculationService,
                Ioc.Resolve<Func<IConfirmActionDialog>>());

            ActivateWindow(window, tank.Id);
        }

        void ITabWindowHost.OpenProductionPredictionTab(Tank tank)
        {
            if (ActivateWindowIfAlreadyOpen<ProductionPredictionTabView>(tank.Id)) return;

            var window = new ProductionPredictionTabView(
                tank,
                _modelStore,
                _eventAggregator,
                _toastNotification,
                _materialBalanceDataReader,
                Ioc.Resolve<ICartesianChartControl>,
                Ioc.Resolve<IChartWrapper>,
                Ioc.Resolve<IRegressionSetupWindow>,
                Ioc.Resolve<IOpenTehraniPlotResultWindow>,
                Ioc.Resolve<IMapper>(),
                _calculationService,
                Ioc.Resolve<Func<IConfirmActionDialog>>());

            ActivateWindow(window, tank.Id);
        }
        void ITabWindowHost.OpenPvtMatchingTab(Tank tank)
        {
            if (ActivateWindowIfAlreadyOpen<PvtMatchingTabWindowView>(tank.Id)) return;

            var window = new PvtMatchingTabWindowView(tank, 
                _modelStore, 
                _eventAggregator, 
                _toastNotification, 
                _materialBalanceDataReader,
                _calculationService,
                Ioc.Resolve<ICartesianChartControl>);
            ActivateWindow(window, tank.Id);
        }

        void ITabWindowHost.OpenTankInputTab(Tank tank)
        {
            if (ActivateWindowIfAlreadyOpen<TankInputTabWindowView>(tank.Id)) return;

            var window = new TankInputTabWindowView(tank,
                _modelStore,
                _eventAggregator,
                _toastNotification,
                _materialBalanceDataReader,
                Ioc.Resolve<IMapper>(),
                Ioc.Resolve<Func<IConfirmActionDialog>>(),
                Ioc.Resolve<Func<ICartesianChartControl>>(),
                Ioc.Resolve<Func<IChartWrapper>>());
            ActivateWindow(window, tank.Id);
        }

        void ActivateWindow(DocumentWindow window, Guid tag)
        {
            window.Tag = tag;
            window.GotFocus += (_, __) => PublishActiveFeatureChanged(window);
            tabControl.Windows.Add(window);
            window.Activate();
        }


        void ITabWindowHost.OpenDashboardWindow()
        {
            var window = new DashboardTabWindowView();
            window.GotFocus += (_, __) => PublishActiveFeatureChanged(window);
            tabControl.Windows.Add(window);
            window.Activate();
        }

        void ITabWindowHost.OpenScriptingWindow()
        {
            var window = new ScriptingTabWindow();
            window.GotFocus += (_, __) => PublishActiveFeatureChanged(window);
            tabControl.Windows.Add(window);
            window.Activate();
        }

        // TODO: Test Should unsubscribe previous window from ribbon events
        void PublishActiveFeatureChanged(DocumentWindow window)
        {
            _activeFeatureViewModel?.UnsubscribeToRibbonEvents();
            _activeFeatureViewModel = window.DataContext as IFeatureWindowViewModel;
            _activeFeatureViewModel?.PublishActiveFeatureChanged();
            _activeFeatureViewModel?.SubscribeToRibbonEvents();
        }

        private IModelStore _modelStore => Ioc.Resolve<IModelStore>();
        private IEventAggregator _eventAggregator => Ioc.Resolve<IEventAggregator>();
        private IToastNotification _toastNotification => Ioc.Resolve<IToastNotification>();
        private IMaterialBalanceDataReader _materialBalanceDataReader => Ioc.Resolve<IMaterialBalanceDataReader>();
        private ICalculationServices _calculationService => Ioc.Resolve<ICalculationServices>();
        private IFeatureWindowViewModel _activeFeatureViewModel;
    }

    internal interface ITabWindowHost
    {
        internal void OpenHistoryMatchingTab(Tank tank);
        internal void OpenProductionPredictionTab(Tank tank);
        internal void OpenPvtMatchingTab(Tank tank);
        internal void OpenTankInputTab(Tank tank);
        internal void OpenDashboardWindow();
        internal void OpenScriptingWindow();
    }
}
