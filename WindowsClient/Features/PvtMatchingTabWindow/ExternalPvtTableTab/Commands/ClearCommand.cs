using Prism.Commands;

namespace WindowsClient.Features.PvtMatchingTabWindow.ExternalPvtTableTab.Commands
{
    internal class ClearCommand : DelegateCommandBase
    {
        protected override bool CanExecute(object parameter) => true;

        protected override void Execute(object parameter)
        {
            if (parameter is not ExternalPvtTableTabViewModel externalPvtTableTabViewModel) return;
            
            externalPvtTableTabViewModel.Tank.PvtData.Clear();
            externalPvtTableTabViewModel.ExternalPvtChanged?.Invoke();
        }
    }
}
