using System;
using Prism.Commands;
using WindowsClient.Shared.UIModels.MaterialBalance;
using System.Linq;
using WindowsClient.Services.Calculation;

namespace WindowsClient.Features.HistoryMatchingTabWindow.TehraniAnalysisTab.RegressionSetup.Commands
{
    internal class RunRegressionCommand : DelegateCommandBase
    {
        public RunRegressionCommand(ICalculationServices calculator, 
            RegressionSetupWindowViewModel setupViewModel)
        {
            this.calculator = calculator;
            _setupViewModel = setupViewModel;
        }
        protected override async void Execute(object parameter)
        {
            RegressionStarted?.Invoke();
            RegressionResult result = null;
            try
            {
                result = await calculator.HistoryMatchingService.SingleTankMatchAsync(
                    _setupViewModel.Tank,
                    _setupViewModel.HistoryMatchingVariables.List.Where(x => x.ToBeOptimized).ToList(),
                    _setupViewModel.ChartControl);

            }
            catch (Exception ex) {
                //TODO: Show generic error: Unable to match history to hide math library errors
            }
            finally
            {
                RunCompleted?.Invoke(result);
            }
        }

        protected override bool CanExecute(object parameter) => !_setupViewModel.IsRunningRegression;
        public event Action<RegressionResult> RunCompleted;
        public event Action RegressionStarted;
        private readonly RegressionSetupWindowViewModel _setupViewModel;
        private readonly ICalculationServices calculator;
    }
}
