using Prism.Events;
using WindowsClient.ApplicationLayout.RibbonTabs.AnalyticsTab.Events.Scripting;
using WindowsClient.ApplicationLayout.TabbedWindowWorkspace.Events;

namespace WindowsClient.ApplicationLayout.TabbedWindowWorkspace
{
    internal class TabbedWindowWorkspaceViewModel
    {
        private readonly IEventAggregator eventAggregator;
        private readonly ITabWindowHost tabHost;

        public TabbedWindowWorkspaceViewModel(IEventAggregator eventAggregator, ITabWindowHost tabHost)
        {
            this.eventAggregator = eventAggregator;
            this.tabHost = tabHost;

            SubscribeToRibbonEvents();
        }

        private void SubscribeToRibbonEvents()
        {
            eventAggregator.GetEvent<OpenHistoryMatchingTabEvent>().Subscribe(tabHost.OpenHistoryMatchingTab);
            eventAggregator.GetEvent<OpenProductionPredictionTabEvent>().Subscribe(tabHost.OpenProductionPredictionTab);
            eventAggregator.GetEvent<OpenPvtMatchingTabEvent>().Subscribe(tabHost.OpenPvtMatchingTab);
            eventAggregator.GetEvent<OpenTankInputTabEvent>().Subscribe(tabHost.OpenTankInputTab);
            eventAggregator.GetEvent<OpenVisualisationTabEvent>().Subscribe(tabHost.OpenDashboardWindow);
            eventAggregator.GetEvent<OpenNewScriptWindowEvent>().Subscribe(tabHost.OpenScriptingWindow);
        }
    }
}
