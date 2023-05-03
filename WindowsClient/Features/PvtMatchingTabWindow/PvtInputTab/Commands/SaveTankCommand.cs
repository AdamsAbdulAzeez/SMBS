using Prism.Commands;
using WindowsClient.Services.Storage;
using WindowsClient.Shared.UIModels.MaterialBalance;

namespace WindowsClient.Features.PvtMatchingTabWindow.PvtInputTab.Commands
{
    internal class SaveTankCommand : DelegateCommandBase
    {
        private readonly Tank _tank;
        private readonly IModelStore _modelStore;

        public SaveTankCommand(Tank tank, IModelStore modelStore)
        {
            _tank = tank;
            _modelStore = modelStore;
        }

        protected override bool CanExecute(object parameter) => true;
        protected override async void Execute(object parameter) => await _modelStore.UpdateTankAsync(_tank);
    }
}
