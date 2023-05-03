using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using WindowsClient.ApplicationLayout.ToastNotification;
using WindowsClient.Features.PvtMatchingTabWindow.PvtInputTab.Commands;
using WindowsClient.Services.Calculation;
using WindowsClient.Services.ExcelImport;
using WindowsClient.Services.Storage;
using WindowsClient.Shared.UIModels.MaterialBalance;

namespace WindowsClient.Features.PvtMatchingTabWindow.PvtInputTab
{
    internal class PvtInputTabViewModel : BindableBase
    {
        public PvtInputTabViewModel(Tank tank,
            IModelStore modelStore,
            IToastNotification toastNotification,
            IMaterialBalanceDataReader materialBalanceDataReader,
            ICalculationServices calculationServices)
        {
            Tank = tank;
            PvtMatchingInputChanged += () => RaisePropertyChanged(nameof(PvtMatchingInput));
            PasteCommand = new PasteCommand(toastNotification);
            ClearCommand = new ClearCommand();
            SaveTankCommand = new SaveTankCommand(tank, modelStore);
            ImportPvtMatchingInputCommand = new ImportPvtMatchingInputCommand(materialBalanceDataReader, toastNotification);
            UseMatching = true;
            //IsEnableCondensateTable = true;
        }

        internal void RefreshTankProperties()
        {
            RaisePropertyChanged(nameof(PvtMatchingInput));
            RaisePropertyChanged(nameof(Tank));
        }


        public bool IsOilTank => Tank.FlowingFluid == FluidType.Oil;
        public bool IsGasTank => Tank.FlowingFluid == FluidType.Gas;
        public bool IsCondensateTank => Tank.FlowingFluid == FluidType.Condensate;

        public Tank Tank { get; }
        public double PbWeight { get; set; }
        public double RsWeight { get; set; }
        public double BoWeight { get; set; }
        public bool ChangeWeightingFactor { get; set; }
        public string ComboAutomaticSelectedCorrelationName { get; set; }
        public string ComboUoAutomaticSelectedCorrelationName { get; set; }
        public PasteCommand PasteCommand { get; }
        public ClearCommand ClearCommand { get; }
        public SaveTankCommand SaveTankCommand { get; }
        public ImportPvtMatchingInputCommand ImportPvtMatchingInputCommand { get; }
        internal Action PvtMatchingInputChanged { get; }
        internal Action TankPropertiesChanged { get; }
        public ObservableCollection<PvtDataRow> PvtMatchingInput => new(Tank.PvtMatchingInput);

        private void SetCondensatTablePvt()
        {
            if (Tank.FlowingFluid == FluidType.Condensate && UseTables)
            {
                GasOilDensityVisibility = true;               
            }
            else
            {
                GasOilDensityVisibility = false;               
            }
        }        
        private bool gasOilDensityVisibility;

        public bool GasOilDensityVisibility
        {
            get { return gasOilDensityVisibility; }
            set
            {
                gasOilDensityVisibility = value;
                RaisePropertyChanged();
            }
        }        

        private bool useTables;
        public bool UseTables
        {
            get => useTables;
            set
            {
                useTables = value;
                RaisePropertyChanged();
                if (UseTables)
                {
                    SetCondensatTablePvt();                   
                    Tank.IsExternalPvt = true;
                    IsEnableCondensateTable = false;
                }
                else
                {                   
                    SetCondensatTablePvt();
                    IsEnableCondensateTable = true;
                }
                UseTablesEvent?.Invoke(value);
            }
        }

        private bool useMatching;
        public bool UseMatching
        {
            get => useMatching;
            set
            {
                useMatching = value;
                RaisePropertyChanged();
                if (value)
                {
                    Tank.IsExternalPvt = false;
                }
                UseTablesEvent?.Invoke(!value);
            }
        }

        private bool isEnableCondensateTable = true;
        public bool IsEnableCondensateTable
        { 
            get => isEnableCondensateTable;
            set
            {
                isEnableCondensateTable = value;
                RaisePropertyChanged();
            }                
        }

        private SeparatorStage selectedSeparatorStage = SeparatorStage.SingleStage;
        public SeparatorStage SelectedSeparatorStage
        {
            get => selectedSeparatorStage;
            set
            {
                selectedSeparatorStage = value;
                Tank.SeparatorConfiguration.SeparatorStage = value;
                RaisePropertyChanged();
                RaiseSeparatorStageChanged();
            }
        }

        private PbRsBoModel selectedPbRsBoModel = PbRsBoModel.Glaso;
        public PbRsBoModel SelectedPbRsBoModel
        {
            get => selectedPbRsBoModel;
            set
            {
                selectedPbRsBoModel = value;
                RaisePropertyChanged();
            }
        }

        private OilViscosityModel selectedOilViscosityModel = OilViscosityModel.Beal_et_al;

        public OilViscosityModel SelectedOilViscosityModel
        {
            get => selectedOilViscosityModel;
            set
            {
                selectedOilViscosityModel = value;
                RaisePropertyChanged();
            }
        }

        public bool IsSingleStage => SelectedSeparatorStage == SeparatorStage.SingleStage;
        public bool IsTwoStage => SelectedSeparatorStage == SeparatorStage.TwoStage;
        public bool IsThreeStage => SelectedSeparatorStage == SeparatorStage.ThreeStage;

        public event Action<bool> UseTablesEvent;

        private void RaiseSeparatorStageChanged()
        {
            RaisePropertyChanged(nameof(IsSingleStage));
            RaisePropertyChanged(nameof(IsTwoStage));
            RaisePropertyChanged(nameof(IsThreeStage));
        }
    }
}
