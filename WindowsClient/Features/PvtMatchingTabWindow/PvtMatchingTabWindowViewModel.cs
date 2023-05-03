using System.Collections.Generic;
using System.Windows.Input;
using Prism.Events;
using WindowsClient.ApplicationLayout.RibbonTabs.ModellingTab.Events;
using WindowsClient.ApplicationLayout.TabbedWindowWorkspace.Events;
using WindowsClient.ApplicationLayout.ToastNotification;
using WindowsClient.Features.PvtMatchingTabWindow.PvtInputTab;
using WindowsClient.Features.PvtMatchingTabWindow.ExternalPvtTableTab;
using WindowsClient.Services.Storage;
using WindowsClient.Services.ExcelImport;
using WindowsClient.Shared.UIModels.MaterialBalance;
using WindowsClient.Services.Calculation;
using WindowsClient.Features.PvtMatchingTabWindow.MatchedParametersTab;
using WindowsClient.Features.PvtMatchingTabWindow.LabPvtTableTab;
using Prism.Mvvm;
using System;
using WindowsClient.Features.PvtMatchingTabWindow.MatchedParametersTab.AllMatchedParametersDialog;
using WindowsClient.Features.PvtMatchingTabWindow.VisualisationTab;
using WindowsClient.Shared.Visualisation.CartesianChart;

namespace WindowsClient.Features.PvtMatchingTabWindow
{
    internal class PvtMatchingTabWindowViewModel : BindableBase, IFeatureWindowViewModel
    {
        private FeatureTabs _activeTab = FeatureTabs.PvtMatchingPvtInputTab;
        private readonly IEventAggregator _eventAggregator;
        public PvtMatchingTabWindowViewModel(
            Tank tank, 
            IModelStore modelStore,
            IEventAggregator aggregator,
            IToastNotification toastNotification, 
            IMaterialBalanceDataReader materialBalanceDataReader,
            ICalculationServices calculationServices,
            Func<ICartesianChartControl> chartControl)
        {
            _eventAggregator = aggregator;
            PvtInputTabViewModel = new PvtInputTabViewModel(tank, 
                modelStore, 
                toastNotification, 
                materialBalanceDataReader,
                calculationServices);

            MatchedParametersTabViewModel = new MatchedParametersTabViewModel(tank,
                calculationServices,
                toastNotification,
                Ioc.Resolve<IAllMatchedParametersView>);

            LabPvtTableTabViewModel = new LabPvtTableTabViewModel(tank, toastNotification);
            ExternalPvtTableTabViewModel = new ExternalPvtTableTabViewModel(tank, toastNotification, materialBalanceDataReader);
            VisualisationTabViewModel = new VisualisationTabViewModel(tank, PvtMatchingResult, chartControl);
            MatchedParametersTabViewModel.PvtMatchCompleted += result => VisualisationTabViewModel.SetPvtMatchingResult(result);

            PvtInputTabViewModel.UseTablesEvent += shoudUse =>
            {
                UseTables = shoudUse;
                ExternalPvtTableTabViewModel.ExternalPvtChanged?.Invoke();       
            };
        }


        public void SubscribeToRibbonEvents()
        {
            var pasteSubscription = _eventAggregator.GetEvent<PastePvtInputTableEvent>()
                .Subscribe(() => (PvtInputTabViewModel.PasteCommand as ICommand).Execute(PvtInputTabViewModel));

            var importPvtMatchingInputData = _eventAggregator.GetEvent<ImportPvtMatchingInputDataEvent>()
                .Subscribe(() => (PvtInputTabViewModel.ImportPvtMatchingInputCommand as ICommand).Execute(PvtInputTabViewModel));

            var importExternalPvtData = _eventAggregator.GetEvent<ImportExternalPvtDataFromExcelEvent>()
                .Subscribe(() => (ExternalPvtTableTabViewModel.ImportExternalPvtInputCommand as ICommand).Execute(ExternalPvtTableTabViewModel));

            _eventSubscriptions.Add(pasteSubscription);
            _eventSubscriptions.Add(importPvtMatchingInputData);
            _eventSubscriptions.Add(importExternalPvtData);
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

        private List<SubscriptionToken> _eventSubscriptions = new ();
        public PvtInputTabViewModel PvtInputTabViewModel { get; }
        public ExternalPvtTableTabViewModel ExternalPvtTableTabViewModel { get; }
        public MatchedParametersTabViewModel MatchedParametersTabViewModel { get; }
        public LabPvtTableTabViewModel LabPvtTableTabViewModel { get; }
        public VisualisationTabViewModel VisualisationTabViewModel { get; }
        public PvtMatchingResult PvtMatchingResult { get; private set; } = new();

        private bool useTables;

        public bool UseTables
        {
            get => useTables;
            set 
            { 
                useTables = value;
                RaisePropertyChanged();
            }
        }


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
