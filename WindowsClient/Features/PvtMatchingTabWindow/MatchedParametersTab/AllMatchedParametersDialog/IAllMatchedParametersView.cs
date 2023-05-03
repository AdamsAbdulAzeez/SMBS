using System;
using WindowsClient.Shared.UIModels.MaterialBalance;

namespace WindowsClient.Features.PvtMatchingTabWindow.MatchedParametersTab.AllMatchedParametersDialog
{
    internal interface IAllMatchedParametersView
    {
        void ShowAllMatchedParameters(PvtMatchingResult pvtMatchingResult,
            OilViscosityModel selectedOilViscosityModel, PbRsBoModel selectedPbRsBoModel);

        event Action<PbRsBoModel> PbRsBoModelChanged;
        event Action<OilViscosityModel> OilViscosityModelChanged;
    }
}
