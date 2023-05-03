using Prism.Commands;
using WindowsClient.Shared.UIModels.MaterialBalance;
namespace WindowsClient.Features.CreateModelWindow.Commands
{
    internal class RemoveTankCommand : DelegateCommandBase
    {
        public RemoveTankCommand(CreateModelViewModel viewModel) => ViewModel = viewModel;


        protected override void Execute(object parameter)
        {
            if (parameter is not Tank tank) return;
            ViewModel.Model.Tanks.Remove(tank);
        }

        protected CreateModelViewModel ViewModel { get; }
        protected override bool CanExecute(object newTankName) => true;      
    }
}
