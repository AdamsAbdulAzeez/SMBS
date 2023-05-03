using System.Collections.Generic;
using System.Linq;
using Prism.Commands;
using Prism.Events;
using WindowsClient.ApplicationLayout.TabbedWindowWorkspace.Events;
using WindowsClient.Features;

namespace WindowsClient.ApplicationLayout.RibbonTabs
{
    internal class RibbonButtonCommand<TEventType> : DelegateCommandBase where TEventType : PubSubEvent, new()
    {
        public RibbonButtonCommand(
            IEventAggregator eventAggregator,
            IEnumerable<FeatureTabs> validForFeatures)
        {
            _eventAggregator = eventAggregator;
            _validForFeatures = validForFeatures.ToHashSet();

            eventAggregator.GetEvent<ActiveFeatureChanged>()
                .Subscribe(RaiseCanButtonExecuteChanged);
        }

        public RibbonButtonCommand(IEventAggregator eventAggregator,
            FeatureTabs validForFeature)
        {
            _eventAggregator = eventAggregator;
            _validForFeatures = new HashSet<FeatureTabs> { validForFeature };
            eventAggregator.GetEvent<ActiveFeatureChanged>()
                .Subscribe(RaiseCanButtonExecuteChanged);
        }

        public void RaiseCanButtonExecuteChanged(FeatureTabs feature)
        {
            _activeFeature = feature;
            RaiseCanExecuteChanged();
        }

        protected override void Execute(object parameter) => _eventAggregator.GetEvent<TEventType>().Publish();

        protected override bool CanExecute(object parameter) => 
            _validForFeatures.Contains(_activeFeature)
            || _validForFeatures.Count == 1 && _validForFeatures.Single() == FeatureTabs.None;

        private FeatureTabs _activeFeature;
        private readonly HashSet<FeatureTabs> _validForFeatures;
        private readonly IEventAggregator _eventAggregator;
    }
}
