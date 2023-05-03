using Prism.Commands;
using System.Collections.Generic;
using WindowsClient.Shared.UIModels.MaterialBalance;

namespace WindowsClient.Features.PvtMatchingTabWindow.LabPvtTableTab.Commands
{
    internal class ClearCommand : DelegateCommandBase
    {
        private readonly LabPvtTableTabViewModel _viewModel;

        public ClearCommand(LabPvtTableTabViewModel labPvtTableTabViewModel)
        {
            _viewModel = labPvtTableTabViewModel;
        }

        protected override bool CanExecute(object parameter) => true;

        protected override void Execute(object parameter)
        {
            _viewModel.Tank.SetPvtMatchingInput(new List<PvtDataRow>());
        }
    }
}
