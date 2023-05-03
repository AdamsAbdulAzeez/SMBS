using Prism.Commands;
using Prism.Events;
using System;
using WindowsClient.ApplicationLayout.StatusBar;
using WindowsClient.ApplicationLayout.ToastNotification;
using WindowsClient.Services.Calculation;

namespace WindowsClient.Features.HistoryMatchingTabWindow.PressureSimulationTab.Commands
{
    internal class RunSimulationCommand : DelegateCommandBase
    {
        public RunSimulationCommand(PressureSimulationViewModel viewModel,
            ICalculationServices calculator, 
            IEventAggregator eventAggregator,
            IToastNotification toastNotification)
        {
            _viewModel = viewModel;
            _calculator = calculator;
            _eventAggregator = eventAggregator;
            _toastNotification = toastNotification;
        }
        protected override bool CanExecute(object parameter) => true;

        protected override async void Execute(object parameter)
        {
            var progressBarEvent = _eventAggregator.GetEvent<ChangeStatusBarMessageEvent>();
            if (!IsValid()) return;
            try
            {
                progressBarEvent.Publish("Running Simulation...");
                var result = await _calculator.PressureSimulationService.SimulateAsync(_viewModel.Tank, _viewModel.DateUpdate, _viewModel.Step);
                progressBarEvent.Publish("Populating table...");
                _viewModel.SetSimulatedPressureResultTable(result);
                _toastNotification.ShowInformation("Pressure simulation complete!", "Pressure Simulation");
            }
            catch (Exception e) { }
            finally
            {
                progressBarEvent.Publish(null);
            }
        }


        private bool IsValid()
        {
            if (_viewModel.Step <= 0)
            {
                _toastNotification.ShowError("Step size should be positive!", "Operation Failed");
                return false;
            }
            return true;
        }

        private readonly PressureSimulationViewModel _viewModel;
        private readonly ICalculationServices _calculator;
        private readonly IEventAggregator _eventAggregator;
        private readonly IToastNotification _toastNotification;
    }
}
