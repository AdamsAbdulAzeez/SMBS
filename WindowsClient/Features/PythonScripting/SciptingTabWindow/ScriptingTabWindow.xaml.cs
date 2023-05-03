using Prism.Events;

namespace WindowsClient.Features.PythonScripting.SciptingTabWindow
{
    /// <summary>
    /// Interaction logic for ScriptingTabWindow.xaml
    /// </summary>
    public partial class ScriptingTabWindow
    {
        public ScriptingTabWindow()
        {
            InitializeComponent();
            DataContext = new ScriptingTabWindowViewModel(Ioc.Resolve<IEventAggregator>(), Terminal);
        }
    }
}
