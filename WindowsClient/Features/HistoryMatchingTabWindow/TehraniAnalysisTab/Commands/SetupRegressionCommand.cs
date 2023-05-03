using System;
using AutoMapper;
using Prism.Commands;
using WindowsClient.Features.HistoryMatchingTabWindow.TehraniAnalysisTab.RegressionSetup;
using WindowsClient.Services.Calculation;
using WindowsClient.Shared.UIModels.MaterialBalance;
using WindowsClient.Shared.Visualisation;

namespace WindowsClient.Features.HistoryMatchingTabWindow.TehraniAnalysisTab.Commands
{
    internal class SetupRegressionCommand : DelegateCommandBase
    {
        public SetupRegressionCommand(
            TehraniAnalysisTabViewModel tabViewModel,
            Func<IRegressionSetupWindow> getSetupWindow,
            IMapper mapper,
            Func<IChartWrapper> getchartControl, 
            ICalculationServices calculator)
        {
            _getWindow = getSetupWindow;
            _getchartControl = getchartControl;
            _calculator = calculator;
            _mapper = mapper;
            AnalysisViewModel = tabViewModel;
        }

        protected override void Execute(object _)
        {
            var window = _getWindow();
            var viewModel = new RegressionSetupWindowViewModel(
                AnalysisViewModel,
                _mapper,
                _getchartControl,
                _calculator);

            viewModel.ResultAccepted += (result) =>
            {
                window.Close();
                ResultAccepted?.Invoke(result);
            };

            window.ShowDialog(viewModel);
        }

        protected override bool CanExecute(object parameter) => true;

        private readonly Func<IRegressionSetupWindow> _getWindow;
        private readonly IMapper _mapper;
        private readonly ICalculationServices _calculator;
        private readonly Func<IChartWrapper> _getchartControl;
        public TehraniAnalysisTabViewModel AnalysisViewModel { get; }
        public event Action<RegressionResult> ResultAccepted;
    }
}
