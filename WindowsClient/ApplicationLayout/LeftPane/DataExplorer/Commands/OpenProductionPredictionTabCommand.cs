using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsClient.ApplicationLayout.TabbedWindowWorkspace.Events;
using WindowsClient.Shared.UIModels.MaterialBalance;

namespace WindowsClient.ApplicationLayout.LeftPane.DataExplorer.Commands
{
    internal class OpenProductionPredictionTabCommand : DelegateCommandBase
    {
        public OpenProductionPredictionTabCommand(IEventAggregator eventAggregator) => _eventAggregator = eventAggregator;


        protected override void Execute(object parameter) => _eventAggregator
            .GetEvent<OpenProductionPredictionTabEvent>()
            .Publish(parameter as Tank);

        protected override bool CanExecute(object parameter) => true;
        private readonly IEventAggregator _eventAggregator;
    }
}
