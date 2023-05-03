using System;
using System.Windows;
using WindowsClient.Shared.UIModels.MaterialBalance;

namespace WindowsClient.Features.PvtMatchingTabWindow.MatchedParametersTab.AllMatchedParametersDialog
{
    /// <summary>
    /// Interaction logic for MatchedParametersTabView.xaml
    /// </summary>
    public partial class AllMatchedParametersView : Window, IAllMatchedParametersView
    {
        public AllMatchedParametersView()
        {
            InitializeComponent();
            DataContext = new AllMatchedParametersViewModel(this);
        }

        public event Action<PbRsBoModel> PbRsBoModelChanged;
        public event Action<OilViscosityModel> OilViscosityModelChanged;

        public void ShowAllMatchedParameters(PvtMatchingResult pvtMatchingResult,
            OilViscosityModel selectedOilViscosityModel, 
            PbRsBoModel selectedPbRsBoModel)
        {
            var dataContext = DataContext as AllMatchedParametersViewModel;
            dataContext.OilViscosityModelChanged += oi => OilViscosityModelChanged?.Invoke(oi);
            dataContext.PbRsBoModelModelChanged += pb => PbRsBoModelChanged?.Invoke(pb);
            dataContext.SetPvtMatchingResult(pvtMatchingResult, selectedOilViscosityModel, selectedPbRsBoModel);
            Owner = Application.Current.MainWindow;
            Show();
        }
    }
}
