using System;
using Prism.Commands;
using WindowsClient.Shared.UIModels.MaterialBalance;
using WindowsClient.Shared.UserInteractions;

namespace WindowsClient.Features.HistoryMatchingTabWindow.TehraniAnalysisTab.Commands
{
    internal class DeleteResultCommand : DelegateCommandBase
    {
        private readonly TehraniAnalysisTabViewModel _viewModel;
        private readonly Func<IConfirmActionDialog> _getConfirmActionDialog;

        public DeleteResultCommand(TehraniAnalysisTabViewModel viewModel,
            Func<IConfirmActionDialog> getConfirmActionDialog)
        {
            _viewModel = viewModel;
            _getConfirmActionDialog = getConfirmActionDialog;
        }

        protected override bool CanExecute(object parameter) => true;

        protected override void Execute(object parameter)
        {
            if (parameter is not RegressionResult regressionResult) return;

            _getConfirmActionDialog().Confirm("Do you want to delete the result?", () =>
            {
                if (regressionResult == null) return;
                int selectedIndex = _viewModel.Chart.RegressionResults.IndexOf(regressionResult);
                _viewModel.Chart.RegressionResults.Remove(regressionResult);
                _viewModel.Tank.RegressionResults.Remove(regressionResult);
                if (_viewModel.Chart.RegressionResults.Count == 0)
                {
                    ClearAllCharts?.Invoke(null);
                    return;
                }
                if (selectedIndex >= _viewModel.Chart.RegressionResults.Count)
                {
                    selectedIndex = _viewModel.Chart.RegressionResults.Count - 1;
                    _viewModel.Chart.SelectedRegressionResult = _viewModel.Chart.RegressionResults[selectedIndex];
                }
                else
                {
                    _viewModel.Chart.SelectedRegressionResult = _viewModel.Chart.RegressionResults[selectedIndex];
                }
            });
        }

        public event Action<RegressionResult> ClearAllCharts;
    }
}
