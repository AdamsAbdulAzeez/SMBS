using Prism.Commands;
using WindowsClient.Features.CreateModelWindow;
using WindowsClient.Shared.UIModels.MaterialBalance;

namespace WindowsClient.Features.CreateModelWindow.Commands
{
    internal class AddTankCommand : DelegateCommandBase
    {
        public AddTankCommand(CreateModelViewModel createModelViewModel) => ViewModel = createModelViewModel;

        protected override void Execute(object parameter)
        {
            var tank = new Tank
            {
                Name = "New Tank",
                FlowingFluid = FluidType.Oil,
                ModelId = ViewModel.Model.Id
            };

            ViewModel.Model.Tanks.Add(tank);
        }

        protected override bool CanExecute(object newTankName) => true;
        private CreateModelViewModel ViewModel { get; }
    }
}
