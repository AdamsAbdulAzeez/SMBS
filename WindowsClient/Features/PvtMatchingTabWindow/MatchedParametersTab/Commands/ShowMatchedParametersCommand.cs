using Prism.Commands;
using System;
using WindowsClient.Features.PvtMatchingTabWindow.MatchedParametersTab.AllMatchedParametersDialog;

namespace WindowsClient.Features.PvtMatchingTabWindow.MatchedParametersTab.Commands
{
    internal class ShowMatchedParametersCommand : DelegateCommandBase
    {
        private readonly MatchedParametersTabViewModel _viewModel;
        private readonly Func<IAllMatchedParametersView> _getAllMatchedParamsView;

        public ShowMatchedParametersCommand(MatchedParametersTabViewModel matchedParametersTabViewModel,
            Func<IAllMatchedParametersView> getAllMatchedParamsView)
        {
            _viewModel = matchedParametersTabViewModel;
            _getAllMatchedParamsView = getAllMatchedParamsView;
        }

        protected override bool CanExecute(object parameter) => true;

        protected override void Execute(object parameter)
        {

            var view = _getAllMatchedParamsView();
            view.PbRsBoModelChanged += pb => _viewModel.SelectedPbRsBoModel = pb;
            view.OilViscosityModelChanged += oi => _viewModel.SelectedOilViscosityModel = oi;
            view.ShowAllMatchedParameters(_viewModel.PvtMatchingResult,
                    _viewModel.SelectedOilViscosityModel,
                    _viewModel.SelectedPbRsBoModel);
        }
    }
}
