using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using WindowsClient.ApplicationLayout.ToastNotification;
using WindowsClient.Features.PvtMatchingTabWindow.LabPvtTableTab.Commands;
using WindowsClient.Shared.UIModels.MaterialBalance;

namespace WindowsClient.Features.PvtMatchingTabWindow.LabPvtTableTab
{
    internal class LabPvtTableTabViewModel : BindableBase
    {
        public LabPvtTableTabViewModel(Tank tank,
            IToastNotification toastNotification)
        {
            Tank = tank;
            ClearCommand = new ClearCommand(this);
            PasteCommand = new PasteCommand(this, toastNotification);
            Tank.PvtMatchingInputChanged += () => RaisePropertyChanged(nameof(PvtMatchingInput));
        }

        public bool IsOilTank => Tank.FlowingFluid == FluidType.Oil;
        public bool IsGasTank => Tank.FlowingFluid == FluidType.Gas;
        public bool IsCondensateTank => Tank.FlowingFluid == FluidType.Condensate;

        public Tank Tank { get; }
        public ClearCommand ClearCommand { get; }
        public PasteCommand PasteCommand { get; }

        public ObservableCollection<PvtDataRow> PvtMatchingInput => new(Tank.PvtMatchingInput);
    }
}
