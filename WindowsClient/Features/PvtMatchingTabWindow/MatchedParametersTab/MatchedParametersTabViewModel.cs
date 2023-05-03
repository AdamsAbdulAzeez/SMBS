using Prism.Mvvm;
using System;
using System.Linq;
using WindowsClient.ApplicationLayout.ToastNotification;
using WindowsClient.Features.PvtMatchingTabWindow.MatchedParametersTab.AllMatchedParametersDialog;
using WindowsClient.Features.PvtMatchingTabWindow.MatchedParametersTab.Commands;
using WindowsClient.Services.Calculation;
using WindowsClient.Shared.UIModels.MaterialBalance;
using PVTLibrary;

namespace WindowsClient.Features.PvtMatchingTabWindow.MatchedParametersTab
{
    internal class MatchedParametersTabViewModel : BindableBase
    {

        public MatchedParametersTabViewModel(Tank tank,
            ICalculationServices calculationServices,
            IToastNotification toastNotification,
            Func<IAllMatchedParametersView> getAllMatchedParametersView)
        {
            Tank = tank;
            MatchCommand = new MatchCommand(this,
                toastNotification,
                calculationServices);
            ShowMatchedParametersCommand = new ShowMatchedParametersCommand(this, getAllMatchedParametersView);
        }

        public Tank Tank { get; }
        public bool IsOilTank => Tank.FlowingFluid == FluidType.Oil;
        public MatchCommand MatchCommand { get; }
        public ShowMatchedParametersCommand ShowMatchedParametersCommand { get; }
        internal PvtMatchingResult PvtMatchingResult
        {
            get => pvtMatchingResult;
            set
            {
                pvtMatchingResult = value;
                PvtMatchCompleted?.Invoke(value);
                SetPvtData();
            }
        }

        internal event Action<PvtMatchingResult> PvtMatchCompleted;

        public MatchingParameters PbParameters => PvtMatchingResult.GetPbResultFor(SelectedPbRsBoModel);
        public MatchingParameters RsParameters => PvtMatchingResult.GetRsResultFor(SelectedPbRsBoModel);
        public MatchingParameters BoParameters => PvtMatchingResult.GetBoResultFor(SelectedPbRsBoModel);
        public MatchingParameters UoParameters => PvtMatchingResult.GetUoResultFor(SelectedOilViscosityModel);
        public MatchingParameters BgParameters => PvtMatchingResult.GetBgResultFor();
        public MatchingParameters GasViscosityParameters => PvtMatchingResult.GetGasViscosityResultFor();

        private bool isMatching;
        public bool IsMatching
        {
            get => isMatching;
            set
            {
                isMatching = value;
                RaisePropertyChanged();
            }
        }

        private PbRsBoModel selectedPbRsBoModel;
        public PbRsBoModel SelectedPbRsBoModel
        {
            get => selectedPbRsBoModel;
            set
            {
                if (selectedPbRsBoModel == value) return;
                selectedPbRsBoModel = value;
                Tank.PbRsBoModel = selectedPbRsBoModel;

                Tank.MatchedPVT.ChoosenModels = ((Enums.PbRsBoModel)SelectedPbRsBoModel, (Enums.OilViscosityModel)SelectedOilViscosityModel,
                     Enums.GasCompressibilityModel.HallYarbourough, Enums.GasViscosityModel.Lee_et_al);

                RaisePropertyChanged();
                RaiseSelectedPbRsBoModelChanged();
                SetPvtData();
            }
        }
        

        private OilViscosityModel selectedOilViscosityModel;
        private PvtMatchingResult pvtMatchingResult = new();

        public OilViscosityModel SelectedOilViscosityModel
        {
            get => selectedOilViscosityModel;
            set
            {
                if (selectedOilViscosityModel == value) return;
                selectedOilViscosityModel = value;
                Tank.OilViscosityModel = selectedOilViscosityModel;

                Tank.MatchedPVT.ChoosenModels = ((Enums.PbRsBoModel)SelectedPbRsBoModel, (Enums.OilViscosityModel)SelectedOilViscosityModel,
                     Enums.GasCompressibilityModel.HallYarbourough, Enums.GasViscosityModel.Lee_et_al);

                RaisePropertyChanged();
                RaiseOilViscosityChanged();
                SetPvtData();
            }
        }

        internal void RaiseGasViscosityAndBgChanged()
        {
            RaisePropertyChanged(nameof(GasViscosityParameters));
            RaisePropertyChanged(nameof(BgParameters));
        }

        internal void RaiseOilViscosityChanged()
        {
            RaisePropertyChanged(nameof(UoParameters));
        }

        internal void RaiseSelectedPbRsBoModelChanged()
        {
            RaisePropertyChanged(nameof(PbParameters));
            RaisePropertyChanged(nameof(RsParameters));
            RaisePropertyChanged(nameof(BoParameters));
        }

        private void SetPvtData()
           => Tank.PvtData = PvtMatchingResult.GeneratedPvtData.
                FirstOrDefault(d => d.OilViscosityModel == selectedOilViscosityModel && d.PbRsBoModel == selectedPbRsBoModel).Data;
    }
}
