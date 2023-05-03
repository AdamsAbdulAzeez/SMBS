using System.Timers;

namespace WindowsClient.ApplicationLayout.ToastNotification
{
    /// <summary>
    /// Interaction logic for ErrorNotification.xaml
    /// </summary>
    public partial class ErrorNotification
    {
        public ErrorNotification(string errorMessage, string title)
        {
            InitializeComponent();
            txtTitle.Text = title;
            txtMessage.Text = errorMessage;
            var autoCloseTimer = new Timer(2000);
            autoCloseTimer.Elapsed += (_, __) => Dispatcher.Invoke(Close);
            autoCloseTimer.Start();
        }
    }
}
