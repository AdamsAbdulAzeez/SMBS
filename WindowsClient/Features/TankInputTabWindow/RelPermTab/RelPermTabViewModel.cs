using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using WindowsClient.Features.TankInputTabWindow.RelPermTab.Commands;
using WindowsClient.Shared.UIModels.MaterialBalance;
using WindowsClient.Shared.Visualisation.CartesianChart;

namespace WindowsClient.Features.TankInputTabWindow.RelPermTab
{
    public class RelPermTabViewModel : BindableBase
    {
        private bool showTable;

        public RelPermTabViewModel(Tank tank, Func<ICartesianChartControl> getChartControl)
        {
            Chart = getChartControl();
            SetDefaultChart();
            Tank = tank;
            ShowTableCommand = new DelegateCommand(() => ShowTable = true, () => !ShowTable);
            ShowPlotCommand = new DelegateCommand(() => ShowTable = false, () => ShowTable);
            PlotCommand = new PlotCommand(this);
            ClearChartCommand = new DelegateCommand(() => { Chart.Clear(); SetDefaultChart(); });
            ShowTable = true;
            SetRelPermDataTable(tank.RelPermData);
            Tank.CalculateRelativePermeabilityPlotData();
        }

        private void SetDefaultChart()
        {
            Chart.SetTitle("Relative Permeabilities Plot");
            Chart.SetPrimaryXAxisTitle("Phase Saturation (fraction)");
            Chart.SetPrimaryXRange(new Shared.Visualisation.CartesianChart.Range(0, 1));
            Chart.SetPrimaryYRange(new Shared.Visualisation.CartesianChart.Range(0, 1));
        }

        public bool ShowTable
        {
            get => showTable; set
            {
                showTable = value;
                RaisePropertyChanged();
                ShowPlotCommand?.RaiseCanExecuteChanged();
                ShowTableCommand?.RaiseCanExecuteChanged();
            }
        }

        private void SetRelPermDataTable(RelativePermeabilityData relativePermeabilityData)
        {
            RelativePermeabilityData.Add(relativePermeabilityData.WaterRelPerm);
            RelativePermeabilityData.Add(relativePermeabilityData.OilRelPerm);
            RelativePermeabilityData.Add(relativePermeabilityData.GasRelPerm);
        }

        public DelegateCommand ShowTableCommand { get; }
        public DelegateCommand ShowPlotCommand { get; }
        public DelegateCommand ClearChartCommand { get; }
        public PlotCommand PlotCommand { get; }
        public Tank Tank { get; }
        public ObservableCollection<RelativePermeabilityDataRow> RelativePermeabilityData { get; set; } = new();
        public ICartesianChartControl Chart { get; }
    }
}
