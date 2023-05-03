using Prism.Mvvm;
using System;
using WindowsClient.ApplicationLayout.ToastNotification;
using WindowsClient.Services.ExcelImport;
using WindowsClient.Services.Storage;
using WindowsClient.Shared.UIModels.MaterialBalance;

namespace WindowsClient.Features.TankInputTabWindow.WaterInfluxTab
{
    internal class WaterInfluxTabViewModel : BindableBase
    {
        public WaterInfluxTabViewModel(Tank tank, 
            IModelStore modelStore, 
            IToastNotification toastNotification,
            IMaterialBalanceDataReader materialBalanceDataReader)
        {
            Tank = tank;
            Tank.TankAquiferChanged += OnTankAquiferChanged;
            Tank.Aquifer.Configuration.GeometryChanged += () => RaisePropertyChanged(nameof(IsRadial));
            Tank.Aquifer.Configuration.GeometryChanged += () => RaisePropertyChanged(nameof(IsLinear));
            Tank.Aquifer.Configuration.PositionChanged += () => RaisePropertyChanged(nameof(IsBottom));
            Tank.Aquifer.ModelTypeChanged += () => RaisePropertyChanged(nameof(IsNone));
        }

        private void OnTankAquiferChanged()
        {
            Tank.Aquifer.Configuration.GeometryChanged += () => RaisePropertyChanged(nameof(IsRadial));
            Tank.Aquifer.Configuration.GeometryChanged += () => RaisePropertyChanged(nameof(IsLinear));
            Tank.Aquifer.Configuration.PositionChanged += () => RaisePropertyChanged(nameof(IsBottom));
            Tank.Aquifer.ModelTypeChanged += () => RaisePropertyChanged(nameof(IsNone));
            RaisePropertyChanged(nameof(Tank));
        }

        public bool IsRadial => Tank.Aquifer.Configuration.Geometry == Geometry.Radial;
        public bool IsLinear => Tank.Aquifer.Configuration.Geometry == Geometry.Linear;
        public bool IsBottom => Tank.Aquifer.Configuration.Position == Position.Bottom;
        public bool IsNone => Tank.Aquifer.ModelType == WaterInfluxModel.None;
        public bool IsOilTank => Tank.FlowingFluid == FluidType.Oil;
        public Tank Tank { get;}
    }
}
