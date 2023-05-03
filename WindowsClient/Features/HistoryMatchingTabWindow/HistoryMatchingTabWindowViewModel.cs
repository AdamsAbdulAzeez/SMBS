using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using AutoMapper;
using Prism.Events;
using WindowsClient.ApplicationLayout.RibbonTabs.ModellingTab.Events;
using WindowsClient.ApplicationLayout.TabbedWindowWorkspace.Events;
using WindowsClient.ApplicationLayout.ToastNotification;
using WindowsClient.Features.HistoryMatchingTabWindow.AllPlots;
using WindowsClient.Features.HistoryMatchingTabWindow.FractionalFlowTab;
using WindowsClient.Features.HistoryMatchingTabWindow.PressureSimulationTab;
using WindowsClient.Features.HistoryMatchingTabWindow.TehraniAnalysisTab;
using WindowsClient.Features.HistoryMatchingTabWindow.TehraniAnalysisTab.RegressionSetup;
using WindowsClient.Features.HistoryMatchingTabWindow.TehraniAnalysisTab.TehraniPlot;
using WindowsClient.Features.HistoryMatchingTabWindow.WdPlot;
using WindowsClient.Services.Calculation;
using WindowsClient.Services.ExcelImport;
using WindowsClient.Services.Storage;
using WindowsClient.Shared.UIModels.MaterialBalance;
using WindowsClient.Shared.UserInteractions;
using WindowsClient.Shared.Visualisation;
using WindowsClient.Shared.Visualisation.CartesianChart;

namespace WindowsClient.Features.HistoryMatchingTabWindow
{
    internal class HistoryMatchingTabWindowViewModel : IFeatureWindowViewModel
    {
        private FeatureTabs _activeTab = FeatureTabs.HistoryMatchingTehraniPlotTab;
        private readonly IEventAggregator _eventAggregator;
        private readonly ICalculationServices _calculator;

        public HistoryMatchingTabWindowViewModel(Tank tank,
            IModelStore modelStore,
            IEventAggregator aggregator,
            IToastNotification toastNotification,
            IMaterialBalanceDataReader materialBalanceDataReader,
            Func<ICartesianChartControl> getCartesianChartControl,
            Func<IChartWrapper> getChartWrapperControl,
            Func<IRegressionSetupWindow> getTehraniSetupWindow,
            Func<IOpenTehraniPlotResultWindow> getTehraniOpenResultWindow,
            IMapper mapper,
            ICalculationServices calculator,
            Func<IConfirmActionDialog> confirmActionDialog)
        {
            Tank = tank;
            _eventAggregator = aggregator;
            _calculator = calculator;
            WdPlotTabViewModel = new WdPlotTabViewModel(getCartesianChartControl(), tank);
             
            PressureSimulationViewModel = new PressureSimulationViewModel(tank, 
                calculator, 
                aggregator,
                toastNotification,
                getChartWrapperControl);

            TehraniAnalysisTabViewModel = new TehraniAnalysisTabViewModel(tank,
                getChartWrapperControl, 
                getTehraniSetupWindow,
                getTehraniOpenResultWindow,
                mapper,
                calculator,
                confirmActionDialog, 
                toastNotification);
            TehraniAnalysisTabViewModel.Chart.ResultAccepted += OnRegressionResultAccepted;
            TehraniAnalysisTabViewModel.DeleteResultCommand.ClearAllCharts += DeleteResultCommand_ClearAllCharts;
            PressureSimulationViewModel.SimulationResult += OnSimulationResult;
            AllPlotsTabViewModel = new AllPlotsTabViewModel(getChartWrapperControl(), getCartesianChartControl(), tank);
            EnergyPlotViewModel = new EnergyPlotViewModel(getChartWrapperControl(), tank);
            GraphicalMethodTabViewModel = new GraphicalMethodTabViewModel(getChartWrapperControl(), tank);
            FractionalFlowTabViewModel = new FractionalFlowTabViewModel(tank, getCartesianChartControl, calculator, toastNotification);

            if (TehraniAnalysisTabViewModel.Chart.RegressionResults.Count > 0)
                TehraniAnalysisTabViewModel.Chart.SelectedRegressionResult = TehraniAnalysisTabViewModel.Chart.RegressionResults.FirstOrDefault();
        }

        private void DeleteResultCommand_ClearAllCharts(RegressionResult obj)
        {
            TehraniAnalysisTabViewModel.Chart.RefreshPlot();

            WdPlotTabViewModel.ChartControl.Clear();
            EnergyPlotViewModel.ChartControl.RefreshPlot();
            GraphicalMethodTabViewModel.ChartControl.RefreshPlot();
            AllPlotsTabViewModel.ChartControl.Clear();
            AllPlotsTabViewModel.SimulationControl.Clear();
            AllPlotsTabViewModel.TehraniControl.RefreshPlot();
            AllPlotsTabViewModel.GraphicalControl.RefreshPlot();
            AllPlotsTabViewModel.EnergyControl.RefreshPlot();
        }

        private void OnSimulationResult(IList<PressureSimulationResultRow> result)
        {
            AllPlotsTabViewModel.PlotSimulationChart(result);
        }

        private void OnRegressionResultAccepted(RegressionResult result)
        {
            if (result.HistoryMatchingVariables == null) return;
            Tank.AcceptedRegressionResult = result;
            if (!Tank.RegressionResults.Any(x =>x.Id == result.Id))
                Tank.RegressionResults.Add(result);
            UpdateTankVariables(Tank, result.HistoryMatchingVariables);
            WdPlotTabViewModel.Refresh(result);
            EnergyPlotViewModel.PlotChart(result.ProductionEstimationResult);
            GraphicalMethodTabViewModel.SetPlotData(result.ProductionEstimationResult);
            AllPlotsTabViewModel.PlotCharts(result, result.ProductionEstimationResult);
        }

        private static void UpdateTankVariables(Tank tank, HistoryMatchingVariables variables)
        {
            tank.Thickness.CurrentValue.DisplayValue = variables.Thickness.BestFitValue;
            tank.STOIP.CurrentValue.DisplayValue = variables.STOIP.BestFitValue;
            tank.GasCap.CurrentValue.DisplayValue = variables.GasCap.BestFitValue;
            tank.GIIP.CurrentValue.DisplayValue = variables.GIIP.BestFitValue;
            tank.Length.CurrentValue.DisplayValue = variables.Length.BestFitValue;
            tank.Radius.CurrentValue.DisplayValue = variables.Radius.BestFitValue;
            tank.Rock.Anisotropy.CurrentValue.DisplayValue = variables.Anisotropy.BestFitValue;
            tank.Rock.Permeability.CurrentValue.DisplayValue = variables.Permeability.BestFitValue;
            tank.Rock.Porosity.CurrentValue.DisplayValue = variables.Porosity.BestFitValue;
            tank.Aquifer.OuterInnerRadiusRatio.CurrentValue.DisplayValue = variables.OuterInnerRadius.BestFitValue;
            tank.Aquifer.EncroachmentAngle.CurrentValue.DisplayValue = variables.EncroachmentAngle.BestFitValue;
            tank.Aquifer.Volume.CurrentValue.DisplayValue = variables.Volume.BestFitValue;
            tank.Width.CurrentValue.DisplayValue = variables.Width.BestFitValue;
        }

        public void SubscribeToRibbonEvents()
        {
            var setupRegressionEvent = _eventAggregator.GetEvent<SetupRegressionEvent>()
                .Subscribe(() => (TehraniAnalysisTabViewModel.SetupRegressionCommand as ICommand).Execute(null));

            var runSimulationEvent = _eventAggregator.GetEvent<RunSimulationEvent>()
                .Subscribe(() => (PressureSimulationViewModel.RunSimulationCommand as ICommand).Execute(null));

            _eventSubscriptions.Add(setupRegressionEvent);
            _eventSubscriptions.Add(runSimulationEvent);
        }

        //TODO: Test case should dispose previous subscription when focus changes.
        public void UnsubscribeToRibbonEvents()
        {
            _eventSubscriptions.ForEach(subscription => subscription.Dispose());
            _eventSubscriptions = new List<SubscriptionToken>();
        }

        public void PublishActiveFeatureChanged()
        {
            _eventAggregator
                .GetEvent<ActiveFeatureChanged>()
                .Publish(_activeTab);
        }

        private List<SubscriptionToken> _eventSubscriptions = new();
        public Tank Tank { get; }
        public WdPlotTabViewModel WdPlotTabViewModel { get; }
        public TehraniAnalysisTabViewModel TehraniAnalysisTabViewModel { get; }
        public PressureSimulationViewModel PressureSimulationViewModel { get; }
        public AllPlotsTabViewModel AllPlotsTabViewModel { get; }
        public EnergyPlotViewModel EnergyPlotViewModel { get; }
        public GraphicalMethodTabViewModel GraphicalMethodTabViewModel { get; }
        public FractionalFlowTabViewModel FractionalFlowTabViewModel { get; }
        //TODO Test: Should publish activeFeature changed when tab changes.
        public FeatureTabs ActiveTab
        {
            get => _activeTab;
            set
            {
                _activeTab = value;
                PublishActiveFeatureChanged();
            }
        }
    }
}
