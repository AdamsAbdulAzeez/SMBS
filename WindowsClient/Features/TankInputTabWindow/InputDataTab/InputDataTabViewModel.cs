using Prism.Mvvm;
using System.Collections.ObjectModel;
using WindowsClient.ApplicationLayout.ToastNotification;
using WindowsClient.Features.TankInputTabWindow.InputDataTab.Commands;
using WindowsClient.Services.ExcelImport;
using WindowsClient.Shared.UIModels.MaterialBalance;

namespace WindowsClient.Features.TankInputTabWindow.InputDataTab
{
    internal class InputDataTabViewModel : BindableBase
    {
        public InputDataTabViewModel(Tank tank, 
            IToastNotification toastNotification,
            IMaterialBalanceDataReader materialBalanceDataReader)
        {
            Tank = tank;
            ImportTankInputDataCommand = new ImportTankInputDataCommand(this, materialBalanceDataReader, toastNotification);
        }

        public void RefreshTankProperties()
        {
            RaisePropertyChanged(nameof(Tank));
            RaisePropertyChanged(nameof(RelativePermeabilityData));
        }

        public bool IsOilTank => Tank.FlowingFluid == FluidType.Oil;
        public Tank Tank { get;}
        public ImportTankInputDataCommand ImportTankInputDataCommand { get; }
        public ObservableCollection<RelativePermeabilityDataRow> RelativePermeabilityData
            => GetRelativePermeabilityData(Tank);

        private ObservableCollection<RelativePermeabilityDataRow> GetRelativePermeabilityData(Tank tank)
        {
            return new ObservableCollection<RelativePermeabilityDataRow>
            {
                tank.RelPermData.OilRelPerm,
                tank.RelPermData.GasRelPerm,
                tank.RelPermData.WaterRelPerm
            };
        }

        private double _rockCompressibility;
        public double RockCompressibility
        {
            get { return _rockCompressibility; }
            set 
            { 
                _rockCompressibility = value; 
                RaisePropertyChanged(); 
            }
        }

        private bool _isUseCorrelationChecked;
        public bool IsUseCorrelationChecked
        {
            get { return _isUseCorrelationChecked; }
            set 
            { 
                _isUseCorrelationChecked = value;
                RaisePropertyChanged();
                RockCompressibility = Tank.EvaluateCompressibility(Tank.Rock.Porosity.CurrentValue);                
            }
        }
    }
}
