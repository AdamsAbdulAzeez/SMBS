using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WindowsClient.ApplicationLayout.ToastNotification;
using WindowsClient.Features.HistoryMatchingTabWindow.PressureSimulationTab.Commands;
using WindowsClient.Services.Calculation;
using WindowsClient.Shared.UIModels.MaterialBalance;
using WindowsClient.Shared.Visualisation;

namespace WindowsClient.Features.HistoryMatchingTabWindow.PressureSimulationTab
{
    internal class PressureSimulationViewModel : BindableBase
    {
        public PressureSimulationViewModel(Tank tank,
            ICalculationServices calculator,
            IEventAggregator eventAggregator,
            IToastNotification toastNotification,
            Func<IChartWrapper> getChartWrapperControl)
        {
            Tank = tank;
            RunSimulationCommand = new RunSimulationCommand(this, calculator, eventAggregator, toastNotification);
            ShowTableCommand = new DelegateCommand(() => ShowTable = true, () => !ShowTable);
            ShowPlotCommand = new DelegateCommand(() => ShowTable = false, () => ShowTable);
            SetHistorySelectedCommand = new DelegateCommand<bool?>(SetHistorySelected);
            ShowTable = true;
            InitializeVaraibles();
            HistoryIsSelected = true;
            ChartControl = getChartWrapperControl();
            PlotVariableCommand = new PlotVariableCommand(this, ChartControl);
            ClearChartCommand = new DelegateCommand(() => PlotVariableCommand.RefreshPlot());
        }

        private void SetHistorySelected(bool? isHistorySelected)
        {
            HistoryIsSelected = isHistorySelected.Value;
            SimulationIsSelected = !HistoryIsSelected;
        }

        internal void SetSimulatedPressureResultTable(IList<PressureSimulationResultRow> pressureSimulationResultRows)
        {
            Tank.PressureSimulationData.Clear();
            Tank.PressureSimulationData.AddRange(pressureSimulationResultRows);
            RaisePropertyChanged(nameof(SimulatedPressureResultTable));
            SimulationResult.Invoke(pressureSimulationResultRows);
        }

        private void InitializeVaraibles()
        {
            HistoryVariableTypes = new ObservableCollection<HistoryVariableModelType>()
            {
                new HistoryVariableModelType() { VariableName = HistoryVariableType.Pressure, IsEnabled = true },
                new HistoryVariableModelType() { VariableName = HistoryVariableType.CummulativeOilProduction, IsEnabled = true },
                new HistoryVariableModelType() { VariableName = HistoryVariableType.CummulativeGasProduction, IsEnabled = true },
                new HistoryVariableModelType() { VariableName = HistoryVariableType.CummulativeWaterInjection, IsEnabled = true },
                new HistoryVariableModelType() { VariableName = HistoryVariableType.CummulativeGasInjection, IsEnabled = true },
                new HistoryVariableModelType() { VariableName = HistoryVariableType.CummulativeWaterProduction, IsEnabled = true },

            };

            SimulationVariableTypes = new ObservableCollection<SimulationVariableModelType>()
            {
                new SimulationVariableModelType(){ VariableName = SimulationVariableType.Pressure, IsEnabled = true },
                new SimulationVariableModelType(){ VariableName = SimulationVariableType.AquiferInflux, IsEnabled = true },
                new SimulationVariableModelType(){ VariableName = SimulationVariableType.AverageGasInjectionRate, IsEnabled = true },
                new SimulationVariableModelType(){ VariableName = SimulationVariableType.AverageGasRate, IsEnabled = true },
                new SimulationVariableModelType(){ VariableName = SimulationVariableType.AverageLiquidRate, IsEnabled = true },
                new SimulationVariableModelType(){ VariableName = SimulationVariableType.AverageOilRate, IsEnabled = true },
                new SimulationVariableModelType(){ VariableName = SimulationVariableType.AverageWaterInjectionRate, IsEnabled = true },
                new SimulationVariableModelType(){ VariableName = SimulationVariableType.AverageWaterRate, IsEnabled = true },
                new SimulationVariableModelType(){ VariableName = SimulationVariableType.CummulativeGasInjection, IsEnabled = true },
                new SimulationVariableModelType(){ VariableName = SimulationVariableType.CummulativeGasProduction, IsEnabled = true },
                new SimulationVariableModelType(){ VariableName = SimulationVariableType.CummulativeOilProduction, IsEnabled = true },
                new SimulationVariableModelType(){ VariableName = SimulationVariableType.CummulativeWaterInjection, IsEnabled = true },
                new SimulationVariableModelType(){ VariableName = SimulationVariableType.OilDensity, IsEnabled = true },
                new SimulationVariableModelType(){ VariableName = SimulationVariableType.OilFVF, IsEnabled = true },
                new SimulationVariableModelType(){ VariableName = SimulationVariableType.OilRecoveryFactor, IsEnabled = true },
                new SimulationVariableModelType(){ VariableName = SimulationVariableType.OilViscosity, IsEnabled = true },
                new SimulationVariableModelType(){ VariableName = SimulationVariableType.ProducingCGR, IsEnabled = true },
                new SimulationVariableModelType(){ VariableName = SimulationVariableType.ProducingGOR, IsEnabled = true },
                new SimulationVariableModelType(){ VariableName = SimulationVariableType.OilWaterContact, IsEnabled = true },
                new SimulationVariableModelType(){ VariableName = SimulationVariableType.GasOilContact, IsEnabled = true },
            };
        }

        public Tank Tank { get; }
        public ObservableCollection<PressureSimulationResultRow> SimulatedPressureResultTable  => new(Tank.PressureSimulationData);
        public RunSimulationCommand RunSimulationCommand { get; }
        public PlotVariableCommand PlotVariableCommand { get; }
        public DelegateCommand ShowTableCommand { get; }
        public DelegateCommand ClearChartCommand { get; }
        public DelegateCommand ShowPlotCommand { get; }
        public DelegateCommand<bool?> SetHistorySelectedCommand { get; }
        public DateUpdate DateUpdate { get; set; } = DateUpdate.Monthly;
        public int Step { get; set; } = 1;
        public IChartWrapper ChartControl { get; }
        public bool IsOilTank => Tank.FlowingFluid == FluidType.Oil;
        public bool IsGasTank => Tank.FlowingFluid == FluidType.Gas;
        public bool IsCondensateTank => Tank.FlowingFluid == FluidType.Condensate;

        private bool showTable;
        public bool ShowTable
        {
            get => showTable;
            set
            {
                showTable = value;
                RaisePropertyChanged();
                ShowPlotCommand.RaiseCanExecuteChanged();
                ShowTableCommand.RaiseCanExecuteChanged();
            }
        }

        private bool historyIsSelected;
        public bool HistoryIsSelected
        {
            get => historyIsSelected;
            set 
            { 
                historyIsSelected = value;
                RaisePropertyChanged();
            }
        }
        private bool simulationIsSelected;
        public bool SimulationIsSelected
        {
            get => simulationIsSelected;
            set
            {
                simulationIsSelected = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<SimulationVariableModelType> SimulationVariableTypes { get; set; }
        public ObservableCollection<HistoryVariableModelType> HistoryVariableTypes { get; set; }

        public event Action<IList<PressureSimulationResultRow>> SimulationResult;
    }
}
