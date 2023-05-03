using System;
using System.Threading.Tasks;
using Prism.Commands;
using WindowsClient.Services.Storage;
using WindowsClient.Shared.UIModels.MaterialBalance;
using WindowsClient.Shared.UserInteractions;

namespace WindowsClient.ApplicationLayout.LeftPane.DataExplorer.Commands
{
    internal class DeleteModelCommand : DelegateCommandBase
    {

        public DeleteModelCommand(IModelStore modelStore, Func<IConfirmActionDialog> confirmationDialog)
        {
            _modelStore = modelStore;
            _getConfirmationDialog = confirmationDialog;
        }
        protected override bool CanExecute(object parameter) => true;

        //TODO: Add error handling and loading indication
        protected override void Execute(object parameter)
        {
            var model = parameter as Model;

            _getConfirmationDialog().Confirm(
                $"Are you sure you want to delete {model.Name.ToUpper()}? Deleting a model cannot be undone.",
                async () => await OnUserConfirmDelete(model)
            );
        }

        private async Task OnUserConfirmDelete(Model model) => await _modelStore.DeleteModelAsync(model);

        private readonly IModelStore _modelStore;
        private readonly Func<IConfirmActionDialog> _getConfirmationDialog;
    }
}
