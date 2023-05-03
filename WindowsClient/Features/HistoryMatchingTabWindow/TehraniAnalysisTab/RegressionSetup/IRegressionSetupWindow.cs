using WindowsClient.Shared.UIModels.MaterialBalance;

namespace WindowsClient.Features.HistoryMatchingTabWindow.TehraniAnalysisTab.RegressionSetup
{
    internal interface IRegressionSetupWindow
    {
        void ShowDialog(RegressionSetupWindowViewModel viewModel);
        void Close();
    }
}
