using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsClient.Features.HistoryMatchingTabWindow.TehraniAnalysisTab.TehraniPlot
{
    interface IOpenTehraniPlotResultWindow
    {
        void ShowDialog(OpenTehraniPlotResultWindowViewModel viewModel);
        void Close();
    }
}
