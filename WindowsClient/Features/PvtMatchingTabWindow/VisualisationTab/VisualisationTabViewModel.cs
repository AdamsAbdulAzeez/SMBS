using Prism.Commands;
using System;
using WindowsClient.Features.PvtMatchingTabWindow.VisualisationTab.Commands;
using WindowsClient.Shared.UIModels.MaterialBalance;
using WindowsClient.Shared.Visualisation.CartesianChart;

namespace WindowsClient.Features.PvtMatchingTabWindow.VisualisationTab
{
    internal class VisualisationTabViewModel
    {
        public VisualisationTabViewModel(Tank tank, PvtMatchingResult pvtMatchingResult,
            Func<ICartesianChartControl> getChartControl)
        {
            Tank = tank;
            PvtMatchingResult = pvtMatchingResult;
            Chart = getChartControl();
            Chart.SetTitle("PVT Plot");
            Chart.SetPrimaryXAxisTitle("Pressure (psia)");
            //Chart.SetPrimaryYAxisTitle("");
            PlotCommand = new PlotCommand(this);
            ClearChartCommand = new DelegateCommand(() => Chart.Clear());
        }

        public ICartesianChartControl Chart { get; }
        public Tank Tank { get; }
        public PvtMatchingResult PvtMatchingResult { get; internal set; }
        public PlotCommand PlotCommand { get;  }
        public DelegateCommand ClearChartCommand { get; }

        private ExtendedOilViscosityModel selectedOilViscosityModel;
        public ExtendedOilViscosityModel SelectedOilViscosityModel
        {
            get => selectedOilViscosityModel;
            set => selectedOilViscosityModel = value;
        }

        private ExtendedPbRsBoModel selectedPbRsBoModel;
        public ExtendedPbRsBoModel SelectedPbRsBoModel
        { 
            get => selectedPbRsBoModel; 
            set => selectedPbRsBoModel = value;
        }

        internal void SetPvtMatchingResult(PvtMatchingResult result) => PvtMatchingResult = result;
    }
}
