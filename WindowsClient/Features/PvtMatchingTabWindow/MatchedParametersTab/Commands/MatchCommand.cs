using Prism.Commands;
using System;
using System.Threading.Tasks;
using WindowsClient.ApplicationLayout.ToastNotification;
using WindowsClient.Services.Calculation;

namespace WindowsClient.Features.PvtMatchingTabWindow.MatchedParametersTab.Commands
{
    internal class MatchCommand : DelegateCommandBase
    {
        private readonly MatchedParametersTabViewModel _viewModel;
        private readonly IToastNotification _toastNotification;
        private readonly ICalculationServices _calculationServices;

        public MatchCommand(MatchedParametersTabViewModel matchedParametersTabViewModel,
            IToastNotification toastNotification,
            ICalculationServices calculationServices)
        {
            _viewModel = matchedParametersTabViewModel;
            _toastNotification = toastNotification;
            _calculationServices = calculationServices;
        }

        
        protected override bool CanExecute(object parameter) => true;

        protected override async void Execute(object parameter)
        {
            try
            {
                _viewModel.IsMatching = true;
                _viewModel.PvtMatchingResult = await _calculationServices.PvtMatchingService.MatchAsync(_viewModel.Tank);
                _viewModel.SelectedOilViscosityModel = _viewModel.PvtMatchingResult.AutoSelected.oilViscosityModel;
                _viewModel.SelectedPbRsBoModel = _viewModel.PvtMatchingResult.AutoSelected.pbRsBoModel;
                _viewModel.RaiseOilViscosityChanged();
                _viewModel.RaiseGasViscosityAndBgChanged();
                _viewModel.RaiseSelectedPbRsBoModelChanged();
                _toastNotification.ShowInformation("", "Match Complete");
            }
            catch (Exception e)
            {
                _toastNotification.ShowError("Failed to match Lab PVT data.", "Operation Failed");
            }
            finally { _viewModel.IsMatching = false; }
        }
    }
}
