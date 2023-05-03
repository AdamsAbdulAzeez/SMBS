using Prism.Events;
using WindowsClient.ApplicationLayout.RibbonTabs.AnalyticsTab.Events.Dashboards;
using WindowsClient.ApplicationLayout.RibbonTabs.AnalyticsTab.Events.Scripting;
using WindowsClient.ApplicationLayout.TabbedWindowWorkspace.Events;
using WindowsClient.Features;

namespace WindowsClient.ApplicationLayout.RibbonTabs.AnalyticsTab
{
    internal class AnalyticsTabViewModel
    {
        public AnalyticsTabViewModel(IEventAggregator eventAggregator)
        {
            // ScriptingCommands
            OpenNewScriptWindowCommand = new RibbonButtonCommand<OpenNewScriptWindowEvent>(eventAggregator, FeatureTabs.None);
            RunScriptCommand = new RibbonButtonCommand<RunScriptEvent>(eventAggregator, FeatureTabs.PythonScriptingTab);

            // DashboardCommands
            var dashboardRelatedFeatures = new[] { FeatureTabs.BarPlotDashboardTab, FeatureTabs.CartesianDashboardTab };
            CreateDashboardCommand = new RibbonButtonCommand<OpenVisualisationTabEvent>(eventAggregator, FeatureTabs.None);
            AddCartesianPageCommand = new RibbonButtonCommand<AddCartesianPageEvent>(eventAggregator, dashboardRelatedFeatures);

            ConfigureCartesianAxesCommand = new RibbonButtonCommand<ConfigureCartesianAxesEvent>(eventAggregator, FeatureTabs.CartesianDashboardTab);

            AddCartesianSeriesCommand = new RibbonButtonCommand<AddCartesianSeriesEvent>(eventAggregator, FeatureTabs.CartesianDashboardTab);
            AddAnnotationCommand = new RibbonButtonCommand<AddCartesianAnnotationEvent>(eventAggregator, FeatureTabs.CartesianDashboardTab);
            ManageSeriesCommand = new RibbonButtonCommand<ManageCartesianSeriesEvent>(eventAggregator, FeatureTabs.CartesianDashboardTab);


            ConfigureBarPlotCommand = new RibbonButtonCommand<ManageCartesianSeriesEvent>(eventAggregator, FeatureTabs.BarPlotDashboardTab);
            AddBarChartPageCommand = new RibbonButtonCommand<ManageCartesianSeriesEvent>(eventAggregator, FeatureTabs.BarPlotDashboardTab);
            CleanPageCommand = new RibbonButtonCommand<CleanActivePageEvent>(eventAggregator, dashboardRelatedFeatures);
        }

        public RibbonButtonCommand<AddCartesianPageEvent> AddCartesianPageCommand { get; }

        public RibbonButtonCommand<OpenVisualisationTabEvent> CreateDashboardCommand { get; }
        public RibbonButtonCommand<OpenNewScriptWindowEvent> OpenNewScriptWindowCommand { get; }
        public RibbonButtonCommand<ConfigureCartesianAxesEvent> ConfigureCartesianAxesCommand { get; }
        public RibbonButtonCommand<AddCartesianSeriesEvent> AddCartesianSeriesCommand { get; }
        public RibbonButtonCommand<AddCartesianAnnotationEvent> AddAnnotationCommand { get; }
        public RibbonButtonCommand<ManageCartesianSeriesEvent> ManageSeriesCommand { get; }
        public RibbonButtonCommand<ManageCartesianSeriesEvent> ConfigureBarPlotCommand { get; }
        public RibbonButtonCommand<ManageCartesianSeriesEvent> AddBarChartPageCommand { get; }

        public RibbonButtonCommand<CleanActivePageEvent> CleanPageCommand { get; }
        public RibbonButtonCommand<RunScriptEvent> RunScriptCommand { get; }
    }
}
