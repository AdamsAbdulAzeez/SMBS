using System.IO;
using System.Windows;
using AutoMapper;
using CypherCrescent.Units.Services;
using Prism.Events;
using Prism.Ioc;
using WindowsClient.ApplicationLayout.ConfirmationDialog;
using WindowsClient.ApplicationLayout.StatusBar;
using WindowsClient.Services.Storage.FileStorage;
using WindowsClient.Services.Storage;
using WindowsClient.ApplicationLayout.ToastNotification;
using WindowsClient.Features.CreateModel;
using WindowsClient.Features.CreateModelWindow;
using WindowsClient.Features.DashboardTabWindow.CartesianPlots.ConfigureAxes;
using WindowsClient.Features.HistoryMatchingTabWindow.TehraniAnalysisTab.RegressionSetup;
using WindowsClient.Services.ExcelImport;
using WindowsClient.Shared.UserInteractions;
using WindowsClient.Features.Transmissibility;
using WindowsClient.Services.Calculation;
using WindowsClient.Shared.Visualisation.CartesianChart;
using WindowsClient.Services.Calculator.HistoryMatching;
using WindowsClient.Services.Calculator.PressureSimulation;
using WindowsClient.Services.Storage.Transformations;
using WindowsClient.Services.Calculation.PvtMatching;
using WindowsClient.Features.PvtMatchingTabWindow.MatchedParametersTab.AllMatchedParametersDialog;
using WindowsClient.Features.HistoryMatchingTabWindow.TehraniAnalysisTab.TehraniPlot;
using WindowsClient.Shared.Visualisation;
using WindowsClient.Features.HistoryMatchingTabWindow.AllPlots;
using WindowsClient.Services.Calculation.FractionalFlowMatching;

namespace WindowsClient
{
    public partial class App
    {
        protected override Window CreateShell() => Container.Resolve<MainWindow>();
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<ICreateModelView, CreateModelWindowView>();
            containerRegistry.Register<IAllMatchedParametersView, AllMatchedParametersView>();
            containerRegistry.Register<IRegressionSetupWindow, RegressionSetupWindow>();
            containerRegistry.Register<IOpenTehraniPlotResultWindow, OpenTehraniPlotResultWindow>();
            containerRegistry.Register<ICartesianChartControl, CartesianChartControl>();
            containerRegistry.Register<IChartWrapper, ChartWrapper>();
            containerRegistry.Register<IConfigureAxesView, ConfigureAxesWindow>();
            containerRegistry.Register<IConfirmActionDialog, ConfirmationDialog>();
            containerRegistry.Register<IMaterialBalanceDataReader, MaterialBalanceDataReader>();
            containerRegistry.Register<IExcelImportDialog, ExcelImportDialog>();

            containerRegistry.Register<IToastNotification, ToastNotificationProvider>();
            containerRegistry.RegisterSingleton<IUnitService, UnitService>();
            containerRegistry.RegisterSingleton<IModelStore>(() => CreateFileStorage);
            containerRegistry.Register<MainWindow>();
            containerRegistry.RegisterSingleton<IMapper>(CreateMapper);
            containerRegistry.Register<ITransmissibilityWindow, TransmissibilityWindow>();
            containerRegistry.Register<IHistoryMatchingService, HistoryMatchingService>();
            containerRegistry.Register<IPressureSimulationService, PressureSimulationService>();
            containerRegistry.Register<IPvtMatchingService, PvtMatchingService>();
            containerRegistry.Register<IFractionalFlowMatchingService, FractionalFlowMatchingService>();
            containerRegistry.RegisterSingleton<ICalculationServices, CalculationServices>();
            containerRegistry.RegisterSingleton<IAllPlotsService, AllPlotsService>();
        }

        protected override async void OnInitialized()
        {
            base.OnInitialized();
            
            Container
                .Resolve<IEventAggregator>()
                    .GetEvent<ChangeStatusBarMessageEvent>()
                    .Publish("Loading application units");

            await Container
                .Resolve<IUnitService>()
                .InitialiseInFlatFileModeAsync(Path.Combine(GetStoragePath(), "Configuration"));

            await Container
                .Resolve<IModelStore>()
                .LoadSavedFilesAsync();
        }

        private string GetStoragePath()
        {
#if DEBUG
            return "C:/ProgramData/SMBS/Dev/";
#else
            return FileStorageDataStore.ProductionStoragePath;
#endif
        }
        private IModelStore CreateFileStorage => new FileStorageDataStore(
            GetStoragePath(),
            Container.Resolve<IEventAggregator>(),
            Container.Resolve<IMapper>());

        private static IMapper CreateMapper() => 
            new MapperConfiguration(
                cfg =>
                {
                    cfg.AddMaps(typeof(TankProfile));
                    cfg.ConstructServicesUsing(type => ContainerLocator.Current.Resolve(type));
                })
            .CreateMapper();
    }
}
