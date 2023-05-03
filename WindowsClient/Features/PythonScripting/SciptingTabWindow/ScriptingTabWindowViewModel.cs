using System.Collections.Generic;
using System.Windows.Documents;
using Prism.Events;
using WindowsClient.ApplicationLayout.RibbonTabs.AnalyticsTab.Events.Scripting;
using WindowsClient.ApplicationLayout.TabbedWindowWorkspace.Events;

namespace WindowsClient.Features.PythonScripting.SciptingTabWindow
{
    internal class ScriptingTabWindowViewModel : IFeatureWindowViewModel
    {
        public ScriptingTabWindowViewModel(IEventAggregator eventAggregator, IPythonTerminal terminal)
        {
            _eventAggregator = eventAggregator;
            _terminal = terminal;
        }

        public void SubscribeToRibbonEvents()
        {
            var runEvent = _eventAggregator
                .GetEvent<RunScriptEvent>()
                .Subscribe(_terminal.Run);

            _eventSubscriptions.Add(runEvent);
        }

        public void UnsubscribeToRibbonEvents()
        {
            _eventSubscriptions.ForEach(subscription => subscription.Dispose());
            _eventSubscriptions.Clear();
        }

        public void PublishActiveFeatureChanged()
        {
            _eventAggregator
                .GetEvent<ActiveFeatureChanged>()
                .Publish(FeatureTabs.PythonScriptingTab);
        }

        private readonly List<SubscriptionToken> _eventSubscriptions = new();
        private readonly IEventAggregator _eventAggregator;
        private readonly IPythonTerminal _terminal;
    }
}
