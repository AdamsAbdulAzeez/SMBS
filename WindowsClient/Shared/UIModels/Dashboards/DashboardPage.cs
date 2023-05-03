using WindowsClient.Features;

namespace WindowsClient.Shared.UIModels.Dashboards
{
    public class DashboardPage
    {
        public DashboardPage() { }

        public DashboardPage(string name) => Name = name;
        public FeatureTabs Feature { get; set; } = FeatureTabs.CartesianDashboardTab;

        public string Name { get; set; }
        public void Clean()
        {
            throw new System.NotImplementedException();
        }

        public void ConfigureCartesianAxes()
        {
            throw new System.NotImplementedException();
        }
    }
}