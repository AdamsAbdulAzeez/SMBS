using System;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using WindowsClient.ApplicationLayout.LeftPane.DataExplorer.Events;
using WindowsClient.ApplicationLayout.RibbonTabs.AnalyticsTab.Events.Scripting;
using WindowsClient.ApplicationLayout.RibbonTabs.ModellingTab.Commands;
using WindowsClient.ApplicationLayout.RibbonTabs.ModellingTab.Events;
using WindowsClient.ApplicationLayout.ToastNotification;
using WindowsClient.Features;
using WindowsClient.Features.CreateModel;
using WindowsClient.Services.Storage;
using WindowsClient.Shared.UIModels.MaterialBalance;

namespace WindowsClient.ApplicationLayout.RibbonTabs.ModellingTab
{
    internal class ModellingTabViewModel : BindableBase
    {
        public ModellingTabViewModel(
            Func<ICreateModelView> getWindow,
            IEventAggregator eventAggregator,
            IToastNotification toastNotification,
            IModelStore modelStore)
        {
            eventAggregator.GetEvent<ModelSelectedEvent>().Subscribe(OnModelSelected);
            PasteProductionDataCommand = new RibbonButtonCommand<PasteProductionDataEvent>(eventAggregator, FeatureTabs.TankInputProductionDataTab);
            PastePvtInputTableCommand = new RibbonButtonCommand<PastePvtInputTableEvent>(eventAggregator, FeatureTabs.LabPvtTab);
            ImportProductionDataCommand = new RibbonButtonCommand<ImportProductionDataEvent>(eventAggregator, FeatureTabs.TankInputProductionDataTab);
            ImportHistoryMatchingInputDataCommand = new RibbonButtonCommand<ImportHistoryMatchingInputDataEvent>(eventAggregator, FeatureTabs.TankInputDataTab);
            ImportPvtMatchingInputDataCommand = new RibbonButtonCommand<ImportPvtMatchingInputDataEvent>(eventAggregator, FeatureTabs.PvtMatchingPvtInputTab);
            CreateModelCommand = new DelegateCommand(() => getWindow().ShowViewAsCreateDialog());

            SetupRegressionCommand = new RibbonButtonCommand<SetupRegressionEvent>(eventAggregator, FeatureTabs.HistoryMatchingTehraniPlotTab);
            RunSimulationCommand = 
                new RibbonButtonCommand<RunSimulationEvent>(eventAggregator,
                new[]
                {
                    FeatureTabs.HistoryMatchingSimulationTab,
                });
            ImportExternalPvtDataCommand = new RibbonButtonCommand<ImportExternalPvtDataFromExcelEvent>(eventAggregator, FeatureTabs.PvtMatchingExternalPvtDataTab);
            PasteExternalPvtDataCommand = new RibbonButtonCommand<PasteExternalPvtDataEvent>(eventAggregator, FeatureTabs.PvtMatchingExternalPvtDataTab);
            RunScriptCommand = new RibbonButtonCommand<RunScriptEvent>(eventAggregator, FeatureTabs.PythonScriptingTab);
            OpenNewScriptWindowCommand = new RibbonButtonCommand<OpenNewScriptWindowEvent>(eventAggregator, FeatureTabs.None);

            DownloadTemplateCommand = new DownloadTemplateCommand(toastNotification);
            _modelStore = modelStore;
            ImportCommand = new ImportCommand(_modelStore, toastNotification);
            ExportCommand = new ExportCommand(toastNotification);
            SaveModelCommand = new SaveModelCommand(modelStore, toastNotification);
        }

        //TODO: Remove unused commands and events
        public DelegateCommand CreateModelCommand { get; set; }
        public RibbonButtonCommand<ImportProductionDataEvent> ImportProductionDataCommand { get; }
        public RibbonButtonCommand<PasteProductionDataEvent> PasteProductionDataCommand { get; }
        public RibbonButtonCommand<PastePvtInputTableEvent> PastePvtInputTableCommand { get; }
        public RibbonButtonCommand<ImportHistoryMatchingInputDataEvent> ImportHistoryMatchingInputDataCommand { get; }
        public RibbonButtonCommand<ImportPvtMatchingInputDataEvent> ImportPvtMatchingInputDataCommand { get; }
        public RibbonButtonCommand<SetupRegressionEvent> SetupRegressionCommand { get; }
        public RibbonButtonCommand<RunSimulationEvent> RunSimulationCommand { get; }
        public RibbonButtonCommand<ImportExternalPvtDataFromExcelEvent> ImportExternalPvtDataCommand { get; }
        public RibbonButtonCommand<PasteExternalPvtDataEvent> PasteExternalPvtDataCommand { get; }
        public DownloadTemplateCommand DownloadTemplateCommand { get; }
        public ImportCommand ImportCommand { get; }
        public ExportCommand ExportCommand { get; }
        public SaveModelCommand SaveModelCommand { get; }
        public RibbonButtonCommand<RunScriptEvent> RunScriptCommand { get; }
        public RibbonButtonCommand<OpenNewScriptWindowEvent> OpenNewScriptWindowCommand { get; }
        internal Model SelectedModel {get; set;}

        private IModelStore _modelStore;
        private FeatureTabs _activeWindowFeature = FeatureTabs.None;

        public FeatureTabs ActiveWindowFeature
        {
            get => _activeWindowFeature;
            set
            {
                _activeWindowFeature = value;
                RaisePropertyChanged(nameof(ActiveWindowFeature));
            }
        }

        private async void OnModelSelected(Guid modelId)
        {
            var result = await _modelStore.GetModelAsync(modelId);
            if (result.IsFailure) return;

            SelectedModel = result.Payload;
        }
    }
}
