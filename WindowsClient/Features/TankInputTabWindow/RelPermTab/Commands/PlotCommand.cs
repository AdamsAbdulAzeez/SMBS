using Prism.Commands;
using System.Linq;
using WindowsClient.Shared.UIModels;
using WindowsClient.Shared.UIModels.MaterialBalance;

namespace WindowsClient.Features.TankInputTabWindow.RelPermTab.Commands
{
    public class PlotCommand : DelegateCommandBase
    {
        private RelPermTabViewModel _viewModel;

        public PlotCommand(RelPermTabViewModel viewModel)
            => _viewModel = viewModel;

        protected override bool CanExecute(object parameter) => true;

        protected override void Execute(object parameter)
        {
            if (parameter is not RelativePermPlotType plotType) return;



            switch (plotType)
            {
                case RelativePermPlotType.KrwVsSw:
                    var krwVsSwSeries = new XYDataSeries
                    {
                        Name = "Krw vs Sw",
                        ShowLine = true,
                        LineWidth = 2,
                        YAxisLabel = "(fraction)",
                        XAxisLabel = "Phase Saturation (fraction)"
                    };

                    var krwVsSwData = Enumerable.Zip(_viewModel.Tank.RelativePermeabilityPlot.Krw,
                        _viewModel.Tank.RelativePermeabilityPlot.Sw);
                    krwVsSwData.ToList().ForEach(row => krwVsSwSeries.Add((row.First, row.Second)));

                    _viewModel.Chart.SetPrimaryXAxisTitle("Phase Saturation (fraction)");
                    _viewModel.Chart.AddSeries(new XYDataSeries[] { krwVsSwSeries });
                    break;               

                case RelativePermPlotType.KrgVsSw:
                    var krgVsSwSeries = new XYDataSeries
                    {
                        Name = "Krg vs Sw",
                        ShowLine = true,
                        LineWidth = 2,
                        YAxisLabel = "(fraction)",
                        XAxisLabel = "Phase Saturation (fraction)"
                    };

                    var krgVsSwData = Enumerable.Zip(_viewModel.Tank.RelativePermeabilityPlot.Krg,
                        _viewModel.Tank.RelativePermeabilityPlot.Sw);
                    krgVsSwData.ToList().ForEach(row => krgVsSwSeries.Add((row.First, row.Second)));

                    _viewModel.Chart.SetPrimaryXAxisTitle("Phase Saturation (fraction)");
                    _viewModel.Chart.AddSeries(new XYDataSeries[] { krgVsSwSeries });
                    break;
                case RelativePermPlotType.KroVsSo:
                    var KroVsSoSeries = new XYDataSeries
                    {
                        Name = "Kro vs So",
                        ShowLine = true,
                        LineWidth = 2,
                        YAxisLabel = "(fraction)",
                        XAxisLabel = "Phase Saturation (fraction)"
                    };

                    var KroVsSoData = Enumerable.Zip(_viewModel.Tank.RelativePermeabilityPlot.Kro,
                        _viewModel.Tank.RelativePermeabilityPlot.So);
                    KroVsSoData.ToList().ForEach(row => KroVsSoSeries.Add((row.First, row.Second)));

                    _viewModel.Chart.SetPrimaryXAxisTitle("Phase Saturation (fraction)");
                    _viewModel.Chart.AddSeries(new XYDataSeries[] { KroVsSoSeries });
                    break;
                case RelativePermPlotType.KrgVsSg:
                    var KrgVsSgSeries = new XYDataSeries
                    {
                        Name = "Krg vs Sg",
                        ShowLine = true,
                        LineWidth = 2,
                        YAxisLabel = "(fraction)",
                        XAxisLabel = "Phase Saturation (fraction)"
                    };

                    var KrgVsSgData = Enumerable.Zip(_viewModel.Tank.RelativePermeabilityPlot.Krg,
                        _viewModel.Tank.RelativePermeabilityPlot.Sg);
                    KrgVsSgData.ToList().ForEach(row => KrgVsSgSeries.Add((row.First, row.Second)));

                    _viewModel.Chart.SetPrimaryXAxisTitle("Phase Saturation (fraction)");
                    _viewModel.Chart.AddSeries(new XYDataSeries[] { KrgVsSgSeries });
                    break;
                case RelativePermPlotType.KroVsSw:
                    var KroVsSwSeries = new XYDataSeries
                    {
                        Name = "Kro vs Sw",
                        ShowLine = true,
                        LineWidth = 2,
                        YAxisLabel = "(fraction)",
                        XAxisLabel = "Phase Saturation (fraction)"
                    };

                    var KroVsSwData = Enumerable.Zip(_viewModel.Tank.RelativePermeabilityPlot.Kro,
                        _viewModel.Tank.RelativePermeabilityPlot.Sw);
                    KroVsSwData.ToList().ForEach(row => KroVsSwSeries.Add((row.First, row.Second)));

                    _viewModel.Chart.SetPrimaryXAxisTitle("Phase Saturation (fraction)");
                    _viewModel.Chart.AddSeries(new XYDataSeries[] { KroVsSwSeries });
                    break;
                case RelativePermPlotType.KroVsSg:
                    var KroVsSgSeries = new XYDataSeries
                    {
                        Name = "Kro vs Sg",
                        ShowLine = true,
                        LineWidth = 2,
                        YAxisLabel = "(fraction)",
                        XAxisLabel = "Phase Saturation (fraction)"
                    };

                    var KroVsSgData = Enumerable.Zip(_viewModel.Tank.RelativePermeabilityPlot.Kro,
                        _viewModel.Tank.RelativePermeabilityPlot.Sg);
                    KroVsSgData.ToList().ForEach(row => KroVsSgSeries.Add((row.First, row.Second)));

                    _viewModel.Chart.SetPrimaryXAxisTitle("Phase Saturation (fraction)");
                    _viewModel.Chart.AddSeries(new XYDataSeries[] { KroVsSgSeries });
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
