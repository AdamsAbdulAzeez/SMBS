using Prism.Commands;
using Prism.Events;
using WindowsClient.ApplicationLayout.TabbedWindowWorkspace.Events;
using WindowsClient.Shared.UIModels.MaterialBalance;

namespace WindowsClient.ApplicationLayout.LeftPane.DataExplorer.Commands
{
    internal class OpenPvtMatchingTabCommand : DelegateCommandBase
    {
        public OpenPvtMatchingTabCommand(IEventAggregator eventAggregator) => _eventAggregator = eventAggregator;

        protected override void Execute(object parameter) => _eventAggregator
            .GetEvent<OpenPvtMatchingTabEvent>()
            .Publish(parameter as Tank);

        protected override bool CanExecute(object parameter) => true;
        private readonly IEventAggregator _eventAggregator;
    }
}