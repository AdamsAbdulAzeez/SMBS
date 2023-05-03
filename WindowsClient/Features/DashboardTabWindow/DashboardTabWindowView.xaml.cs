using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ActiproSoftware.Windows.Controls.Docking;
using Prism.Events;
using WindowsClient.Features.DashboardTabWindow.CartesianPlots.ConfigureAxes;
using WindowsClient.Features.VisualisationTabWindow;
using WindowsClient.Shared.UIModels.Dashboards;
using WindowsClient.Shared.UserInteractions;
using WindowsClient.Shared.Visualisation;
using WindowsClient.Shared.Visualisation.CartesianChart;

namespace WindowsClient.Features.DashboardTabWindow
{
    /// <summary>
    /// Interaction logic for DashboardTabWindowView.xaml
    /// </summary>
    public partial class DashboardTabWindowView : IDashboardPagesControl
    {
        public DashboardTabWindowView()
        {
            InitializeComponent();
            DataContext = new DashboardTabWindowViewModel(
                Ioc.Resolve<IEventAggregator>(),
                Ioc.Resolve<IConfigureAxesView>, 
                this);
        }

        void IDashboardPagesControl.AddPage(DashboardPage page)
        {
            switch (page.Feature)
            {
                case FeatureTabs.CartesianDashboardTab:
                    AddPageAndActivate(page, new CartesianChartControl());
                    break;
                case FeatureTabs.BarPlotDashboardTab:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void AddPageAndActivate(DashboardPage page, UIElement pageContent)
        {
            var tabItem = new AdvancedTabItem
            {
                Content = pageContent,
                Tag = page,
                Header = new PageHeader(page.Name ?? $"Sheet {tabControl.Items.Count + 1}")
            };
            tabControl.Items.Add(tabItem);
            tabControl.SelectedItem = tabItem;

            ActivePageChanged?.Invoke(page);
        }

        void IDashboardPagesControl.RemovePage(int index)
        {
            throw new System.NotImplementedException();
        }

        List<DashboardPage> IDashboardPagesControl.Pages =>
            tabControl.Items.OfType<AdvancedTabItem>().Select(tab => tab.Tag as DashboardPage).ToList();

        public event Action<DashboardPage> ActivePageChanged;
    }
}
