using System.Collections.Generic;
using System.Linq;
using WindowsClient.Shared.UIModels.MaterialBalance;
using WindowsClient.Shared.Visualisation;

namespace WindowsClient.Features.HistoryMatchingTabWindow.WdPlot
{
    internal class GraphicalMethodTabViewModel
    {
        public GraphicalMethodTabViewModel(IChartWrapper chartControl, Tank tank)
        {
            ChartControl = chartControl;
            _tank = tank;
        }

        public void SetPlotData(List<ProductionEstimationResultRow> productionEstimationResult)
        {
            var stoiipOrGiip = "";

            switch (_tank.FlowingFluid)
            {
                case FluidType.Gas:
                case FluidType.Condensate:
                    FoverEt = productionEstimationResult.Select(CalcFOverET).ToList();
                    WeOverEt = productionEstimationResult.Select(CalcWeOverET).ToList();
    
                    stoiipOrGiip = $"GIIP = {_tank.GIIP.CurrentValue.DisplayValue} {_tank.GIIP.CurrentValue.DisplayUnit}";
                    break;

                case FluidType.Oil:
                    FoverEt = productionEstimationResult.Select(CalcFOverET).ToList();
                    WeOverEt = productionEstimationResult.Select(CalcWeOverET).ToList();
   
                    stoiipOrGiip = $"STOIIP = {_tank.STOIP.CurrentValue.DisplayValue} {_tank.STOIP.CurrentValue.DisplayUnit}";
                    break;

            }


            double c = _tank.FlowingFluid == FluidType.Oil ?
                _tank.STOIP.CurrentValue.DisplayValue.Value :
                _tank.GIIP.CurrentValue.DisplayValue.Value;
            FoverEt = FoverEt.Where(foe => foe != 0).ToList();
            WeOverEt = WeOverEt.Where(woe => woe != 0).ToList();
            MathematicsLibrary.ColVec y = FoverEt.Select(yy => yy - c).ToList();
            MathematicsLibrary.ColVec x = WeOverEt;
            double m = y.Transpose() * y / (x.Transpose() * x);
            MathematicsLibrary.ColVec result = new List<double> { m, c };
            XLineOverEt = MathematicsLibrary.ColVec.Linspace(0, WeOverEt.Max(), 100).ToList();
            YLineOverEt = MathematicsLibrary.Optimizers.Trend(XLineOverEt, result, "Linear").ToList();

            PlotChart(stoiipOrGiip);
        }

        private double CalcWeOverET(ProductionEstimationResultRow x)
        {
            var unit = _tank.FlowingFluid == FluidType.Oil ? "MMstb" : "MMscf";
            XTitle = $"We/Et {unit};We/Et";
            return x.Et == 0 ? 0 : x.AquiferInflux / x.Et;
        }

        private double CalcFOverET(ProductionEstimationResultRow x)
        {
            var unit = _tank.FlowingFluid == FluidType.Oil ? "MMstb" : "MMscf";
            YTitle = $"F/Et {unit};F/Et";
            return x.Et == 0 ? 0 : x.F / x.Et;
        }

        private void PlotChart(string stoiipOrGiip)
        {
            ChartControl.Title = $"{_tank.Name} Graphical Plot";
            ChartControl.GetLeftYAxis(1).Title = "Pressure (Psia)";
            ChartControl.GetXAxis().Title = "Cumulative Production (MMSTB)";

            ChartControl.RefreshPlot();
            ChartControl.PlotLineAction(WeOverEt, FoverEt, "F/Et vs We/Et ", XTitle.Split(';')[0], YTitle.Split(';')[0], ConverterUtils.StringToBrush("Red"), 0, 5, XTitle.Split(';')[1], YTitle.Split(';')[1]);
            ChartControl.PlotLineAction(XLineOverEt, YLineOverEt, stoiipOrGiip, XTitle.Split(';')[0], YTitle.Split(';')[0], ConverterUtils.StringToBrush("Blue"), 2, 0, XTitle.Split(';')[1], YTitle.Split(';')[1]);
            ChartControl.ReplotChart();
        }

        private readonly Tank _tank;
        public string XTitle { get; private set; }
        public string YTitle { get; private set; }
        List<double> FoverEt { get; set; } = new List<double>();
        List<double> WeOverEt { get; set; } = new List<double>();
        List<double> YLineOverEt { get; set; } = new List<double>();
        List<double> XLineOverEt { get; set; } = new List<double>();
        public IChartWrapper ChartControl { get; }
    }


}
