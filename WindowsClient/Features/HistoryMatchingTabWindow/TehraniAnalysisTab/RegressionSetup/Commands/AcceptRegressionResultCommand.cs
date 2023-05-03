using Prism.Commands;

namespace WindowsClient.Features.HistoryMatchingTabWindow.TehraniAnalysisTab.RegressionSetup.Commands
{
    internal class AcceptRegressionResultCommand : DelegateCommandBase
    {
        public AcceptRegressionResultCommand(RegressionSetupWindowViewModel setupViewModel)
        {
            SetupViewModel = setupViewModel;
        }

        protected override void Execute(object parameter) => SetupViewModel.AcceptResults();

        protected override bool CanExecute(object parameter) => SetupViewModel.Result != null;
        
        public RegressionSetupWindowViewModel SetupViewModel { get; set; }
    }
}
