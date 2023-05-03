using Prism.Mvvm;
using System;
using WindowsClient.Shared.UIModels.MaterialBalance;

namespace WindowsClient.Features.PvtMatchingTabWindow.MatchedParametersTab.AllMatchedParametersDialog
{
    internal class AllMatchedParametersViewModel : BindableBase
    {

        public AllMatchedParametersViewModel(
            IAllMatchedParametersView view)
        {
            View = view;
        }

        public IAllMatchedParametersView View { get; }
        internal PvtMatchingResult PvtMatchingResult { get; set; } = new();

        public PbRsBoCorrelationResult GlasoPbRsBoCorrelationResult { get; set; } = new();
        public PbRsBoCorrelationResult StandingPbRsBoCorrelationResult { get; set; } = new();
        public PbRsBoCorrelationResult LasaterPbRsBoCorrelationResult { get; set; } = new();
        public PbRsBoCorrelationResult VazquezBeggsPbRsBoCorrelationResult { get; set; } = new();
        public PbRsBoCorrelationResult PetroskyPbRsBoCorrelationResult { get; set; } = new();
        public PbRsBoCorrelationResult AlMarhounPbRsBoCorrelationResult { get; set; } = new();
        public PbRsBoCorrelationResult DeGhettoPbRsBoCorrelationResult { get; set; } = new();
        public MatchingParameters GasViscosityParameters { get; set; } = new();
        public MatchingParameters GasFVFParameters { get; set; } = new();
        public MatchingParameters BealOilViscosityParameters { get; set; } = new();
        public MatchingParameters BeggsOilViscosityParameters { get; set; } = new();
        public MatchingParameters PetroskyOilViscosityParameters { get; set; } = new();
        public MatchingParameters EgbogahOilViscosityParameters { get; set; } = new();
        public MatchingParameters BergmanOilViscosityParameters { get; set; } = new();
        public MatchingParameters DeghettoOilViscosityParameters { get; set; } = new();

        public event Action<OilViscosityModel> OilViscosityModelChanged;
        public event Action<PbRsBoModel> PbRsBoModelModelChanged;

        private PbRsBoModel selectedPbRsBoModel;
        public PbRsBoModel SelectedPbRsBoModel
        {
            get => selectedPbRsBoModel;
            set
            {
                if (selectedPbRsBoModel == value) return;
                selectedPbRsBoModel = value;
                RaisePropertyChanged();
                PbRsBoModelModelChanged?.Invoke(value);
            }
        }

        private OilViscosityModel selectedOilViscosityModel;
        public OilViscosityModel SelectedOilViscosityModel
        {
            get => selectedOilViscosityModel;
            set
            {
                if (selectedOilViscosityModel == value) return;
                selectedOilViscosityModel = value;
                RaisePropertyChanged();
                OilViscosityModelChanged?.Invoke(value);
            }
        }


        internal void SetPvtMatchingResult(PvtMatchingResult pvtMatchingResult,
            OilViscosityModel selectedOilViscosityModel,
            PbRsBoModel selectedPbRsBoModel)
        {
            GlasoPbRsBoCorrelationResult = pvtMatchingResult.GetPbRsBoCorrelationResult(PbRsBoModel.Glaso);
            StandingPbRsBoCorrelationResult = pvtMatchingResult.GetPbRsBoCorrelationResult(PbRsBoModel.Standing);
            LasaterPbRsBoCorrelationResult = pvtMatchingResult.GetPbRsBoCorrelationResult(PbRsBoModel.Lasater);
            VazquezBeggsPbRsBoCorrelationResult = pvtMatchingResult.GetPbRsBoCorrelationResult(PbRsBoModel.Vazquez_Beggs);
            PetroskyPbRsBoCorrelationResult = pvtMatchingResult.GetPbRsBoCorrelationResult(PbRsBoModel.Petrosky_et_al);
            AlMarhounPbRsBoCorrelationResult = pvtMatchingResult.GetPbRsBoCorrelationResult(PbRsBoModel.Al_Marhoun);
            DeGhettoPbRsBoCorrelationResult = pvtMatchingResult.GetPbRsBoCorrelationResult(PbRsBoModel.DeGhetto);

            GasViscosityParameters = pvtMatchingResult.GetGasViscosityResultFor();
            GasFVFParameters = pvtMatchingResult.GetBgResultFor();

            BealOilViscosityParameters = pvtMatchingResult.GetUoResultFor(OilViscosityModel.Beal_et_al);
            BeggsOilViscosityParameters = pvtMatchingResult.GetUoResultFor(OilViscosityModel.Beggs_et_al);
            PetroskyOilViscosityParameters = pvtMatchingResult.GetUoResultFor(OilViscosityModel.Petrosky_et_al);
            EgbogahOilViscosityParameters = pvtMatchingResult.GetUoResultFor(OilViscosityModel.Egbogah_et_al);
            BergmanOilViscosityParameters = pvtMatchingResult.GetUoResultFor(OilViscosityModel.Bergman_Sutton);
            DeghettoOilViscosityParameters = pvtMatchingResult.GetUoResultFor(OilViscosityModel.DeGhetto);

            SelectedPbRsBoModel = selectedPbRsBoModel;
            SelectedOilViscosityModel = selectedOilViscosityModel;
        }
    }
}
