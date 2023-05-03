using Prism.Commands;
using WindowsClient.Features.Transmissibility;
using WindowsClient.Shared.UIModels.MaterialBalance;

namespace WindowsClient.ApplicationLayout.LeftPane.DataExplorer.Commands
{
    class OpenTransmissibilityWindowCommand : DelegateCommandBase
    {
        private readonly ITransmissibilityWindow _transmissibilityWindow;

        public OpenTransmissibilityWindowCommand(ITransmissibilityWindow transmissibilityWindow)
        {
            _transmissibilityWindow = transmissibilityWindow;
        }
        protected override void Execute(object parameter)
        {
            _transmissibilityWindow.OpenDialog();
        }
        protected override bool CanExecute(object parameter)
        {
            if (parameter == null) return false;
            var model = parameter as Model;
            return model.Tanks.Count > 1;
        }
    }
}
