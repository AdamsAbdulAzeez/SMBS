using Prism.Commands;

namespace WindowsClient.Features.PvtMatchingTabWindow.PvtInputTab.Commands
{
    internal class ClearCommand : DelegateCommandBase
    {
        protected override bool CanExecute(object parameter) => true;

        protected override void Execute(object parameter)
        {
            if (parameter is not PvtInputTabViewModel pvtInputTabViewModel) return;

            pvtInputTabViewModel.Tank.PvtMatchingInput.Clear();
            pvtInputTabViewModel.PvtMatchingInputChanged?.Invoke();
        }
    }
}
