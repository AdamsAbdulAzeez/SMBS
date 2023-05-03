using Prism.Commands;

namespace WindowsClient.Features.TankInputTabWindow.ProductionDataTab.Commands
{
    internal class ClearCommand : DelegateCommandBase
    {
        protected override bool CanExecute(object parameter) => true;

        protected override void Execute(object parameter)
        {
            if (parameter is not ProductionDataTabViewModel productionDataTabViewModel) return;

            productionDataTabViewModel.Tank.ProductionData.Clear();
            productionDataTabViewModel.ProductionDataChanged?.Invoke();
        }
    }
}
