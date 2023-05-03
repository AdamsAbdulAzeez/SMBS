using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsClient.Features.HistoryMatchingTabWindow.TehraniAnalysisTab.TehraniPlot;
using WindowsClient.Shared.UIModels.MaterialBalance;
using WindowsClient.Shared.UserInteractions;
using WindowsClient.Shared.Visualisation;

namespace WindowsClient.Features.HistoryMatchingTabWindow.TehraniAnalysisTab.Commands
{
    internal class OpenResultCommand : DelegateCommandBase
    {
        private readonly TehraniAnalysisTabViewModel _viewModel;
        private readonly ObservableCollection<RegressionResult> _results;
        private readonly Func<IOpenTehraniPlotResultWindow> _getResultWindow;
        private readonly Func<IChartWrapper> _getChartControl;

        public OpenResultCommand(TehraniAnalysisTabViewModel viewModel, 
            Func<IChartWrapper> getChartControl,
            Func<IOpenTehraniPlotResultWindow> getResultWindow)
        {
            _viewModel = viewModel;
            _getChartControl = getChartControl;
            _getResultWindow = getResultWindow;
        }

        protected override bool CanExecute(object parameter) => true;

        protected override void Execute(object parameter)
        {
            if (parameter is not RegressionResult regressionResult) return;
            var window = _getResultWindow();
            var viewModel = new OpenTehraniPlotResultWindowViewModel(
                                _viewModel,
                                _getChartControl,
                                regressionResult);
            
            window.ShowDialog(viewModel);
        }
    } 
}
