using System;
using System.Collections.Generic;
using System.Windows.Input;
using AutoMapper;
using Prism.Events;
using WindowsClient.ApplicationLayout.RibbonTabs.ModellingTab.Events;
using WindowsClient.ApplicationLayout.TabbedWindowWorkspace.Events;
using WindowsClient.ApplicationLayout.ToastNotification;
using WindowsClient.Features.TankInputTabWindow.AreaAndPoreVolumeVsDepthTab;
using WindowsClient.Features.TankInputTabWindow.InputDataTab;
using WindowsClient.Features.TankInputTabWindow.ProductionDataTab;
using WindowsClient.Features.TankInputTabWindow.RelPermTab;
using WindowsClient.Features.TankInputTabWindow.WaterInfluxTab;
using WindowsClient.Services.ExcelImport;
using WindowsClient.Services.Storage;
using WindowsClient.Shared.UIModels.MaterialBalance;
using WindowsClient.Shared.UserInteractions;
using WindowsClient.Shared.Visualisation;
using WindowsClient.Shared.Visualisation.CartesianChart;

namespace WindowsClient.Features.TankInputTabWindow
{
    internal class TankInputTabWindowViewModel : IFeatureWindowViewModel
    {
        private FeatureTabs _activeTab = FeatureTabs.TankInputDataTab;
        private readonly IEventAggregator _eventAggregator;

        public TankInputTabWindowViewModel(Tank tank,
            IModelStore modelStore,
            IEventAggregator aggregator,
            IToastNotification toastNotification,
            IMaterialBalanceDataReader materialBalanceDataReader,
            IMapper mapper,
            Func<IConfirmActionDialog> confirmActionDialog,
            Func<ICartesianChartControl> getCartesianChartControl,
            Func<IChartWrapper> getChartControl)
        {
            Tank = tank;
            _eventAggregator = aggregator;
            ProductionDataTabViewModel = new ProductionDataTabViewModel(tank, 
                toastNotification, 
                materialBalanceDataReader,
                getChartControl);

            WaterInfluxTabViewModel = new WaterInfluxTabViewModel(tank, 
                modelStore, 
                toastNotification, 
                materialBalanceDataReader);

            InputDataTabViewModel = new InputDataTabViewModel(tank,
                toastNotification,
                materialBalanceDataReader);

            AreaAndPoreVolumeVsDepthTabViewModel = new AreaAndPoreVolumeVsDepthTabViewModel(tank, toastNotification);
            RelPermTabViewModel = new RelPermTabViewModel(tank, getCartesianChartControl);
        }

        private void OnRegressionResultAccepted(RegressionResult result)
        {
            Tank.AcceptedRegressionResult = result;
            Tank.RegressionResults.Add(result);
        }

        public void SubscribeToRibbonEvents()
        {
            var pasteProductionDataSubscription = _eventAggregator.GetEvent<PasteProductionDataEvent>()
                .Subscribe(() => (ProductionDataTabViewModel.PasteCommand as ICommand)
                .Execute(ProductionDataTabViewModel));

            var importProductionDataSubscription = _eventAggregator.GetEvent<ImportProductionDataEvent>()
                .Subscribe(() => (ProductionDataTabViewModel.ImportProductionDataCommand as ICommand)
                .Execute(ProductionDataTabViewModel));

            var importInputDataSubscription = _eventAggregator.GetEvent<ImportHistoryMatchingInputDataEvent>()
                .Subscribe(() => (InputDataTabViewModel.ImportTankInputDataCommand as ICommand)
                .Execute(null));

            _eventSubscriptions.Add(pasteProductionDataSubscription);
            _eventSubscriptions.Add(importProductionDataSubscription);
            _eventSubscriptions.Add(importInputDataSubscription);
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
        public ProductionDataTabViewModel ProductionDataTabViewModel { get; }
        public WaterInfluxTabViewModel WaterInfluxTabViewModel { get; }
        public InputDataTabViewModel InputDataTabViewModel { get; }
        public AreaAndPoreVolumeVsDepthTabViewModel AreaAndPoreVolumeVsDepthTabViewModel { get; }
        public RelPermTabViewModel RelPermTabViewModel { get; }
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
