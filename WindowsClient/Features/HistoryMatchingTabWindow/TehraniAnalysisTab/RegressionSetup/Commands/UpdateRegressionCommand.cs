using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsClient.Services.Calculation;
using WindowsClient.Shared.UIModels.MaterialBalance;

namespace WindowsClient.Features.HistoryMatchingTabWindow.TehraniAnalysisTab.RegressionSetup.Commands
{
    internal class UpdateRegressionCommand : DelegateCommandBase
    {
        public UpdateRegressionCommand(ICalculationServices calculator,
            RegressionSetupWindowViewModel setupViewModel)
        {
            this.calculator = calculator;
            _setupViewModel = setupViewModel;
        }
        protected override async void Execute(object parameter)
        {
            if (_setupViewModel.Result == null) return;
            var selectedResult = _setupViewModel.ChartControl.SelectedRegressionResult;
            selectedResult.EstimatedNpSeries = _setupViewModel.Result.EstimatedNpSeries;
            EstimateResult result = null;
            try
            {
                _setupViewModel.SetCurrentTankResult();
                _setupViewModel.UpdateTankVariables(_setupViewModel.Tank, _setupViewModel.HistoryMatchingVariables);
                if (selectedResult != null && selectedResult.TurnOffPoints != null && selectedResult.TurnOffPoints.Count > 0)
                {
                    result = await calculator.HistoryMatchingService.SingleTankEstimateAsync(_setupViewModel.Tank, _setupViewModel.ChartControl, selectedResult.TurnOffPoints);
                }
                else
                {
                    result = await calculator.HistoryMatchingService.SingleTankEstimateAsync(_setupViewModel.Tank, _setupViewModel.ChartControl, null);
                }

            }
            catch (Exception ex)
            {
                //TODO: Show generic error: Unable to match history to hide math library errors
            }
            finally
            {
                UpdateCompleted?.Invoke(result);
            }
        }

        protected override bool CanExecute(object parameter) => true;

        public event Action<EstimateResult> UpdateCompleted;
        private readonly RegressionSetupWindowViewModel _setupViewModel;
        private readonly ICalculationServices calculator;
    }
}
