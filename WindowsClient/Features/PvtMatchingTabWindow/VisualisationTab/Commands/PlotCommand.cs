using Prism.Commands;
using System.Linq;
using WindowsClient.Shared.UIModels;
using WindowsClient.Shared.UIModels.MaterialBalance;

namespace WindowsClient.Features.PvtMatchingTabWindow.VisualisationTab.Commands
{
    internal class PlotCommand : DelegateCommandBase
    {
        private VisualisationTabViewModel _viewModel;

        public PlotCommand(VisualisationTabViewModel visualisationTabViewModel)
            => _viewModel = visualisationTabViewModel;

        protected override bool CanExecute(object parameter) => true;

        protected override void Execute(object parameter)
        {
            if (parameter is not PvtPlotType pvtPlotType) return;
            GeneratedPvt pvtData = null;

            switch (pvtPlotType)
            {
                case PvtPlotType.Pressure:
                    var pressureSeries = new XYDataSeries
                    {
                        Name = "Pressure",
                        ShowLine = true,
                        LineWidth = 2,
                        YAxisLabel = "Pressure (psia)",
                        XAxisLabel = "Pressure (psia)"
                    };

                    if (_viewModel.SelectedPbRsBoModel == ExtendedPbRsBoModel.LabData)
                    {
                        _viewModel.Tank.PvtMatchingInput.ForEach(row => pressureSeries.Add((row.Pressure, row.Pressure)));
                        
                    }
                    else
                    {
                         pvtData = _viewModel.PvtMatchingResult.GeneratedPvtData
                            .FirstOrDefault(d => d.OilViscosityModel.ToString() == _viewModel.SelectedOilViscosityModel.ToString());
                    
                        if (pvtData == null) return;

                        pvtData.Data.ForEach(row => pressureSeries.Add((row.Pressure, row.Pressure)));
                    }
                     
                    _viewModel.Chart.SetPrimaryXAxisTitle("Pressure (psia)");
                    _viewModel.Chart.AddSeries(new XYDataSeries[] { pressureSeries});
                    break;
                case PvtPlotType.BubblePoint:
                    var bubblePointSeries = new XYDataSeries
                    {
                        Name = "Bubble Point - " + _viewModel.SelectedPbRsBoModel,
                        ShowLine = true,
                        LineWidth = 2,
                        YAxisLabel = "Bubble Point (psia)",
                        XAxisLabel = "Pressure (psia)"
                    };

                    if (_viewModel.SelectedPbRsBoModel == ExtendedPbRsBoModel.LabData)
                    {
                        _viewModel.Tank.PvtMatchingInput.ForEach(row => bubblePointSeries.Add((row.Pressure, row.BubblePoint)));
                        bubblePointSeries.Name = "Bubble Point - Lab Data";
                        SetMarker(bubblePointSeries);
                    }
                    else
                    {
                        pvtData = _viewModel.PvtMatchingResult.GeneratedPvtData
                            .FirstOrDefault(d => d.PbRsBoModel.ToString() == _viewModel.SelectedPbRsBoModel.ToString());
                        if (pvtData == null) return;

                        pvtData.Data.ForEach(row => bubblePointSeries.Add((row.Pressure, row.BubblePoint)));
                    }
                    _viewModel.Chart.SetPrimaryXAxisTitle("Pressure (psia)");
                    _viewModel.Chart.AddSeries(new XYDataSeries[] { bubblePointSeries });
                    break;
                case PvtPlotType.GasOilRatio:
                    var gasOilRatioSeries = new XYDataSeries
                    {
                        Name = "GOR - " + _viewModel.SelectedPbRsBoModel,
                        ShowLine = true,
                        LineWidth = 2,
                        YAxisLabel = "GOR (scf/stb)",
                        XAxisLabel = "Pressure (psia)"
                    };

                    if (_viewModel.SelectedPbRsBoModel == ExtendedPbRsBoModel.LabData)
                    {
                        _viewModel.Tank.PvtMatchingInput.ForEach(row => gasOilRatioSeries.Add((row.Pressure, row.GasOilRatio)));
                        gasOilRatioSeries.Name = "GOR - Lab Data";
                        SetMarker(gasOilRatioSeries);
                    }
                    else
                    {
                        pvtData = _viewModel.PvtMatchingResult.GeneratedPvtData
                            .FirstOrDefault(d => d.PbRsBoModel.ToString() == _viewModel.SelectedPbRsBoModel.ToString());
                        if (pvtData == null) return;

                        pvtData.Data.ForEach(row => gasOilRatioSeries.Add((row.Pressure, row.GasOilRatio)));
                    }
                    _viewModel.Chart.SetPrimaryXAxisTitle("Pressure (psia)");
                    _viewModel.Chart.AddSeries(new XYDataSeries[] { gasOilRatioSeries });
                    break;
                case PvtPlotType.OilFVF:
                    var oilFVFSeries = new XYDataSeries
                    {
                        Name = "Oil FVF - " + _viewModel.SelectedPbRsBoModel,
                        ShowLine = true,
                        LineWidth = 2,
                        YAxisLabel = "Oil FVF (rb/stb)",
                        XAxisLabel = "Pressure (psia)"
                    };

                    if (_viewModel.SelectedPbRsBoModel == ExtendedPbRsBoModel.LabData)
                    {
                        _viewModel.Tank.PvtMatchingInput.ForEach(row => oilFVFSeries.Add((row.Pressure, row.OilFVF)));
                        oilFVFSeries.ShowLine = false;
                        SetMarker(oilFVFSeries);
                        oilFVFSeries.Name = "Oil FVF - Lab Data";
                    }
                    else
                    {
                        pvtData = _viewModel.PvtMatchingResult.GeneratedPvtData
                            .FirstOrDefault(d => d.PbRsBoModel.ToString() == _viewModel.SelectedPbRsBoModel.ToString());
                        if (pvtData == null) return;

                        pvtData.Data.ForEach(row => oilFVFSeries.Add((row.Pressure, row.OilFVF)));
                    }
                    _viewModel.Chart.SetPrimaryXAxisTitle("Pressure (psia)");
                    _viewModel.Chart.AddSeries(new XYDataSeries[] { oilFVFSeries });
                    break;
                case PvtPlotType.OilViscosity:
                    var oilViscositySeries = new XYDataSeries
                    {
                        Name = "Oil Viscosity - " + _viewModel.SelectedOilViscosityModel,
                        ShowLine = true,
                        LineWidth = 2,
                        YAxisLabel = "Oil Viscosity (cp)",
                        XAxisLabel = "Pressure (psia)"
                    };

                    if (_viewModel.SelectedOilViscosityModel == ExtendedOilViscosityModel.LabData)
                    {
                        _viewModel.Tank.PvtMatchingInput.ForEach(row => oilViscositySeries.Add((row.Pressure, row.OilViscosity)));
                        oilViscositySeries.Name = "Oil Viscosity - Lab Data";
                        SetMarker(oilViscositySeries);
                    }
                    else
                    {
                        pvtData = _viewModel.PvtMatchingResult.GeneratedPvtData
                            .FirstOrDefault(d => (d.OilViscosityModel.ToString() == _viewModel.SelectedOilViscosityModel.ToString() && 
                            d.PbRsBoModel.ToString() == _viewModel.SelectedPbRsBoModel.ToString()));
                        if (pvtData == null) return;
                        pvtData.Data.ForEach(row => oilViscositySeries.Add((row.Pressure, row.OilViscosity)));
                    }
                    _viewModel.Chart.SetPrimaryXAxisTitle("Pressure (psia)");
                    _viewModel.Chart.AddSeries(new XYDataSeries[] { oilViscositySeries });
                    break;
                case PvtPlotType.GasFVF:
                    var gasFVFSeries = new XYDataSeries
                    {
                        Name = "Gas FVF",
                        ShowLine = true,
                        LineWidth = 2,
                        YAxisLabel = "Gas FVF (cuft/scf)",
                        XAxisLabel = "Pressure (psia)"
                    };

                    if (_viewModel.SelectedPbRsBoModel == ExtendedPbRsBoModel.LabData)
                    {
                        _viewModel.Tank.PvtMatchingInput.ForEach(row => gasFVFSeries.Add((row.Pressure, row.GasFVF)));
                        gasFVFSeries.Name = "Gas FVF - Lab Data";
                        SetMarker(gasFVFSeries);
                    }
                    else
                    {
                        pvtData = _viewModel.PvtMatchingResult.GeneratedPvtData
                            .FirstOrDefault(d => d.PbRsBoModel.ToString() == _viewModel.SelectedOilViscosityModel.ToString());
                        if (pvtData == null) return;

                        pvtData.Data.ForEach(row => gasFVFSeries.Add((row.Pressure, row.GasFVF)));
                    }
                    _viewModel.Chart.SetPrimaryXAxisTitle("Pressure (psia)");
                    _viewModel.Chart.AddSeries(new XYDataSeries[] { gasFVFSeries });
                    break;
                case PvtPlotType.GasViscosity:
                    var gasViscositySeries = new XYDataSeries
                    {
                        Name = "Gas Viscosity",
                        ShowLine = true,
                        LineWidth = 2,
                        YAxisLabel = "Gas Viscosity (cp)",
                        XAxisLabel = "Pressure (psia)"
                    };

                    if (_viewModel.SelectedPbRsBoModel == ExtendedPbRsBoModel.LabData)
                    {
                        _viewModel.Tank.PvtMatchingInput.ForEach(row => gasViscositySeries.Add((row.Pressure, row.GasViscosity)));
                        gasViscositySeries.Name = "Gas Viscosity - Lab Data";
                        SetMarker(gasViscositySeries);
                    }
                    else
                    {
                        pvtData = _viewModel.PvtMatchingResult.GeneratedPvtData
                            .FirstOrDefault(d => d.PbRsBoModel.ToString() == _viewModel.SelectedOilViscosityModel.ToString());
                        if (pvtData == null) return;
                        pvtData.Data.ForEach(row => gasViscositySeries.Add((row.Pressure, row.GasViscosity)));
                    }
                    _viewModel.Chart.SetPrimaryXAxisTitle("Pressure (psia)");
                    _viewModel.Chart.AddSeries(new XYDataSeries[] { gasViscositySeries });
                    break;
            }
        }

        private static void SetMarker(XYDataSeries series)
        {
            series.MarkerSize = 7F;
            series.Color = "Black";
            series.ShowLine = false;
        }
    }
}