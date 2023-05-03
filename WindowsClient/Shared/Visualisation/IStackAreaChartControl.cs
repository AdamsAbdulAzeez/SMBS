using System.Collections.Generic;
using WindowsClient.Shared.UIModels;

namespace WindowsClient.Shared.Visualisation
{
    internal interface IStackAreaChartControl
    {
        void Plot(List<XYDataSeries> dataSeries);
    }
}
