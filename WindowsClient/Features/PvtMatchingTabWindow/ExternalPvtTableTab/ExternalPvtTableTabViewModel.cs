using System;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using WindowsClient.ApplicationLayout.ToastNotification;
using WindowsClient.Features.PvtMatchingTabWindow.ExternalPvtTableTab.Commands;
using WindowsClient.Shared.UIModels.MaterialBalance;
using WindowsClient.Services.ExcelImport;

namespace WindowsClient.Features.PvtMatchingTabWindow.ExternalPvtTableTab
{
    internal class ExternalPvtTableTabViewModel : BindableBase
    {
        public ExternalPvtTableTabViewModel(Tank tank, 
            IToastNotification toastNotification,
            IMaterialBalanceDataReader reader)
        {
            Tank = tank;
            ExternalPvtChanged += () => RaisePropertyChanged(nameof(ExternalPvtData));
            PasteCommand = new PasteCommand(toastNotification);
            ImportExternalPvtInputCommand = new ImportExternalPvtInputCommand(reader, toastNotification);
            Tank.FlowingFluidChanged += OnFlowingFluidChanged;
        }

        private void OnFlowingFluidChanged(FluidType _)
        {
            RaisePropertyChanged(nameof(IsOilTank));
            RaisePropertyChanged(nameof(IsGasTank));
            RaisePropertyChanged(nameof(IsCondensateTank));
        }

        public Tank Tank { get; }
        public ObservableCollection<PvtDataRow> ExternalPvtData => new(Tank.PvtData);

        internal Action ExternalPvtChanged { get; }
        public PasteCommand PasteCommand { get; }
        public ClearCommand ClearCommand { get; } = new ClearCommand();
        public ImportExternalPvtInputCommand ImportExternalPvtInputCommand { get; }
        public bool IsOilTank => Tank.FlowingFluid == FluidType.Oil;
        public bool IsGasTank => Tank.FlowingFluid == FluidType.Gas;
        public bool IsCondensateTank => Tank.FlowingFluid == FluidType.Condensate;
    }
}
