using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Prism.Events;
using WindowsClient.ApplicationLayout.RibbonTabs.AnalyticsTab.Events.Dashboards;
using WindowsClient.ApplicationLayout.TabbedWindowWorkspace.Events;
using WindowsClient.Features.DashboardTabWindow;
using WindowsClient.Features.DashboardTabWindow.CartesianPlots.ConfigureAxes;
using WindowsClient.Shared.UIModels.Dashboards;

namespace WindowsClient.Features.VisualisationTabWindow
{
    internal class DashboardTabWindowViewModel : IFeatureWindowViewModel
    {
        public DashboardTabWindowViewModel(
            IEventAggregator eventAggregator,
            Func<IConfigureAxesView> resolveConfigureAxesView,
            IDashboardPagesControl dashboardControl)
        {
            _eventAggregator = eventAggregator;
            _resolveConfigureAxesView = resolveConfigureAxesView;
            _dashboardControl = dashboardControl;
            dashboardControl.ActivePageChanged += OnPageChanged;

            _dashboardControl.AddPage(new DashboardPage());
        }

        public void SubscribeToRibbonEvents()
        {
            var addCartesianPageEvent = _eventAggregator.GetEvent<AddCartesianPageEvent>()
                .Subscribe(() => _dashboardControl.AddPage(new DashboardPage()));

            var cleanActivePage = _eventAggregator.GetEvent<CleanActivePageEvent>()
                .Subscribe(() => ActivePage.Clean());

            var configureCartesianAxes = _eventAggregator.GetEvent<ConfigureCartesianAxesEvent>()
                .Subscribe(ConfigureCartesianAxesForActiveTab);

            var addCartesianSeries = _eventAggregator.GetEvent<AddCartesianSeriesEvent>()
                .Subscribe(() => ActivePage.ConfigureCartesianAxes());

            var addCartesianAnnotation = _eventAggregator.GetEvent<AddCartesianAnnotationEvent>()
                .Subscribe(() => ActivePage.ConfigureCartesianAxes());

            var manageCartesianSeries = _eventAggregator.GetEvent<ManageCartesianSeriesEvent>()
                .Subscribe(() => ActivePage.ConfigureCartesianAxes());

            var configureBarPlot = _eventAggregator.GetEvent<ConfigureBarPlotType>()
                .Subscribe(() => ActivePage.ConfigureCartesianAxes());

            var addBarPlot = _eventAggregator.GetEvent<AddBarPlotSeries>()
                .Subscribe(() => ActivePage.ConfigureCartesianAxes());

            _eventSubscriptions.Add(addCartesianPageEvent);
            _eventSubscriptions.Add(cleanActivePage);
            _eventSubscriptions.Add(configureCartesianAxes);
            _eventSubscriptions.Add(addCartesianSeries);
            _eventSubscriptions.Add(addCartesianAnnotation);
            _eventSubscriptions.Add(manageCartesianSeries);

            _eventSubscriptions.Add(configureBarPlot);
            _eventSubscriptions.Add(addBarPlot);
        }

        public void ConfigureCartesianAxesForActiveTab()
        {
            var configurationView = _resolveConfigureAxesView();
            configurationView.ShowViewAsDialog();
        }

        public void UnsubscribeToRibbonEvents()
        {
            _eventSubscriptions.ForEach(subscription => subscription.Dispose());
            _eventSubscriptions.Clear();
        }

        public void OnPageChanged(DashboardPage activePage) => ActivePage = activePage;

        public void PublishActiveFeatureChanged() =>
            _eventAggregator
                    .GetEvent<ActiveFeatureChanged>()
                    .Publish(ActivePage.Feature);

        private readonly IEventAggregator _eventAggregator;
        private readonly List<SubscriptionToken> _eventSubscriptions = new();
        private DashboardPage _activePage;
        private readonly Func<IConfigureAxesView> _resolveConfigureAxesView;
        private readonly IDashboardPagesControl _dashboardControl;

        public DashboardPage ActivePage
        {
            get => _activePage;
            set
            {
                _activePage = value;
                PublishActiveFeatureChanged();
            }
        }
    }
}
