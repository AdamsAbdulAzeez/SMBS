using Prism.Mvvm;
using WindowsClient.Shared.UIModels.MaterialBalance;
using WindowsClient.Features.TankInputTabWindow.AreaAndPoreVolumeVsDepthTab.Commands;
using WindowsClient.ApplicationLayout.ToastNotification;
using System.Collections.ObjectModel;
using EngineShared = SMBS.Shared.DataImport;

namespace WindowsClient.Features.TankInputTabWindow.AreaAndPoreVolumeVsDepthTab
{
    internal class AreaAndPoreVolumeVsDepthTabViewModel : BindableBase
    {
        public AreaAndPoreVolumeVsDepthTabViewModel(Tank tank, 
            IToastNotification toastNotification)
        {
            Tank = tank;
            PasteCommand = new PasteCommand(this, toastNotification);
            ClearCommand = new ClearCommand(this);
            IsAvd = !Tank.IsPvd;
            IsPvd = Tank.IsPvd;
        }

        public Tank Tank { get; }
        public ObservableCollection<EngineShared.AreaDepthRow> AreaDepthData => new ObservableCollection<EngineShared.AreaDepthRow>(Tank.AreaDepthData);
        public ObservableCollection<EngineShared.PoreVolumeDepthRow> PoreVolumeDepthData => new ObservableCollection<EngineShared.PoreVolumeDepthRow>(Tank.PoreVolumeDepth);
        public PasteCommand PasteCommand { get; }
        public ClearCommand ClearCommand { get; }

        private bool isPvd;
        public bool IsPvd
        {
            get => isPvd;
            set
            {
                isPvd = value;
                Tank.IsPvd = value;
                RaisePropertyChanged();
            }
        }

        private bool isAvd;
        public bool IsAvd
        {
            get => isAvd;
            set
            {
                isAvd = value;
                Tank.IsPvd = !IsAvd;
                RaisePropertyChanged();
            }
        }

        internal void RaiseTankChanged()
        {
            RaisePropertyChanged(nameof(AreaDepthData));
            RaisePropertyChanged(nameof(PoreVolumeDepthData));
        }
    }
}
