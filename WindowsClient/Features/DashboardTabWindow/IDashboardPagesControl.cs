using System;
using System.Collections.Generic;
using WindowsClient.Shared.UIModels.Dashboards;

namespace WindowsClient.Features.DashboardTabWindow
{
    internal interface IDashboardPagesControl
    {
        void AddPage(DashboardPage page);
        void RemovePage(int index);
        List<DashboardPage> Pages { get; }
        public event Action<DashboardPage> ActivePageChanged;
    }
}
