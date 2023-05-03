using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Prism.Mvvm;
using WindowsClient.ApplicationLayout.ToastNotification;
using WindowsClient.Features.HistoryMatchingTabWindow.TehraniAnalysisTab.Commands;
using WindowsClient.Features.HistoryMatchingTabWindow.TehraniAnalysisTab.RegressionSetup;
using WindowsClient.Features.HistoryMatchingTabWindow.TehraniAnalysisTab.TehraniPlot;
using WindowsClient.Services.Calculation;
using WindowsClient.Shared.UIModels.MaterialBalance;
using WindowsClient.Shared.UserInteractions;
using WindowsClient.Shared.Visualisation;

namespace WindowsClient.Features.HistoryMatchingTabWindow.TehraniAnalysisTab
{
    internal class TehraniAnalysisTabViewModel : BindableBase
    {

        public TehraniAnalysisTabViewModel(
            Tank tank,
            Func<IChartWrapper> getChartControl,
            Func<IRegressionSetupWindow> getSetupWindow,
            Func<IOpenTehraniPlotResultWindow> getResultWindow,
            IMapper mapper,
            ICalculationServices calculator,
            Func<IConfirmActionDialog> getConfirmActionDialog,
            IToastNotification toastNotification)
        {
            Tank = tank;
            Chart = getChartControl();
            Chart.tank = tank;

            SetupRegressionCommand = new SetupRegressionCommand(
                this,
                getSetupWindow,
                mapper,
                getChartControl,
                calculator);

            SetupRegressionCommand.ResultAccepted += Chart.OnResultAccepted;
            DeleteResultCommand = new DeleteResultCommand(this, getConfirmActionDialog);
            OpenResultCommand = new OpenResultCommand(this, getChartControl, getResultWindow);
            Chart.RegressionResults.CollectionChanged += (o, _) => RaisePropertyChanged(nameof(ShowNoRegressionCasesWarning));
            LoadChart(tank, calculator).Await();

            _toastNotification = toastNotification;
        }

        private async Task LoadChart(Tank tank, ICalculationServices calculator)
        {
            if (tank.RegressionResults != null && tank.RegressionResults.Count > 0)
            {
                foreach (var item in tank.RegressionResults)
                {
                    Chart.RegressionResults.Add(item);
                }
            }
            else
            {
                Chart.PlotMeasuredNp();
                try
                {
                    var nP = await calculator.HistoryMatchingService.SingleTankEstimateAsync(tank, Chart, null);
                    Chart.EstimatedNpPlot(nP);
                    Chart.PlotSelectedPoints();
                    Chart.ReplotChart();
                }
                catch (Exception e)
                {
                    _toastNotification.ShowError("Input data not set", "Failed to load chart");
                    return;
                }
            }
        }

        public IChartWrapper Chart { get; set; }

        public bool ShowNoRegressionCasesWarning => Chart.RegressionResults.Count == 0;

        public SetupRegressionCommand SetupRegressionCommand { get; set; }
        public DeleteResultCommand DeleteResultCommand { get; }
        public OpenResultCommand OpenResultCommand { get; }
        private IToastNotification _toastNotification;
        //public event Action<RegressionResult> ResultAccepted;
        public Tank Tank { get; }
    }
}
