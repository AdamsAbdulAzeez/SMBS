using System.Linq;
using Prism.Commands;
using WindowsClient.Shared.UIModels.MaterialBalance;

namespace WindowsClient.Features.HistoryMatchingTabWindow.TehraniAnalysisTab.RegressionSetup.Commands
{
    internal class SwapStartAndBestFitCommand : DelegateCommandBase
    {
        public SwapStartAndBestFitCommand(RegressionSetupWindowViewModel setupViewModel) => 
            RegressionSetupViewModel = setupViewModel;

        protected override void Execute(object parameter)
        {
            var result = RegressionSetupViewModel.Result;

            if (parameter is RegressionVariable regressedVariable)
            {
                //regressedVariable.StartValue = result.
                //    .First(variable => variable.Name == regressedVariable.Name).BestFitValue;
            }
        }

        protected override bool CanExecute(object parameter) => true;

        public RegressionSetupWindowViewModel RegressionSetupViewModel { get; }
    }
}
