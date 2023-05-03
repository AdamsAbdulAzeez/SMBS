using Prism.Commands;
using System;
using WindowsClient.ApplicationLayout.ToastNotification;
using WindowsClient.Services.Calculation;
using WindowsClient.Shared.UIModels.MaterialBalance;

namespace WindowsClient.Features.HistoryMatchingTabWindow.FractionalFlowTab.Commands
{
    class RegressCommand : DelegateCommandBase
    {
        private readonly FractionalFlowTabViewModel viewModel;
        private readonly ICalculationServices _calculationsServices;
        private readonly IToastNotification _toastNotification;

        public RegressCommand(FractionalFlowTabViewModel fractionalFlowTabViewModel, 
            ICalculationServices calculationServices,
            IToastNotification toastNotification)
        {
            viewModel = fractionalFlowTabViewModel;
            _calculationsServices = calculationServices;
            _toastNotification = toastNotification;
        }

        protected override bool CanExecute(object parameter) => true;

        protected override async void Execute(object parameter)
        {
            try
            {
                viewModel.Tank.FractionalMatchResult = await _calculationsServices.
                    FractionalFlowMatchingService.
                    MatchAsync(viewModel.Tank, viewModel.FracMatchingChoice);
                _toastNotification.ShowInformation("Fractional flow match completed", "Operation Successful");
            }
            catch(Exception e)
            {
                _toastNotification.ShowError("Failed to match", "Operation failed");
            }
        }
    }
}
