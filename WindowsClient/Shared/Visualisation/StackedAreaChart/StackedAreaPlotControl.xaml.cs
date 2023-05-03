using System.Collections.Generic;
using WindowsClient.Shared.UIModels;
using WindowsClient.Shared.Visualisation.StackedAreaPlot;

namespace WindowsClient.Shared.Visualisation.StackedAreaChart
{
    /// <summary>
    /// Interaction logic for StackedAreaPlot.xaml
    /// </summary>
    public partial class StackedAreaPlot : StackedAreaPlotControl, IStackAreaChartControl
    {
        public StackedAreaPlot()
        {
            InitializeComponent();
        }

        void IStackAreaChartControl.Plot(List<XYDataSeries> dataSeries)
        {
            throw new System.NotImplementedException();
        }
    }
}
