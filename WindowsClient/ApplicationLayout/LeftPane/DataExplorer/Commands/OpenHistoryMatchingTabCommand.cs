using Prism.Commands;
using Prism.Events;
using WindowsClient.ApplicationLayout.TabbedWindowWorkspace.Events;
using WindowsClient.Shared.UIModels.MaterialBalance;

namespace WindowsClient.ApplicationLayout.LeftPane.DataExplorer.Commands
{
    internal class OpenHistoryMatchingTabCommand : DelegateCommandBase
    {
        public OpenHistoryMatchingTabCommand(IEventAggregator eventAggregator) => _eventAggregator = eventAggregator;

        protected override void Execute(object parameter) => _eventAggregator
            .GetEvent<OpenHistoryMatchingTabEvent>()
            .Publish(parameter as Tank);

        protected override bool CanExecute(object parameter) => true;
        private readonly IEventAggregator _eventAggregator;
    }
}