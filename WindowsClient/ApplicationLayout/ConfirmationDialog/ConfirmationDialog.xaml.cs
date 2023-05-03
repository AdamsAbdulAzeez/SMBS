using System;
using System.Windows;
using WindowsClient.Shared.UserInteractions;

namespace WindowsClient.ApplicationLayout.ConfirmationDialog
{
    /// <summary>
    /// Interaction logic for ConfirmationDialog.xaml
    /// </summary>
    public partial class ConfirmationDialog : IConfirmActionDialog
    {
        private Action _onConfirmedCallBack;

        public ConfirmationDialog() => InitializeComponent();

        public void Confirm(string message, Action onActionConfirmed)
        {
            txtMessage.Text = message;
            _onConfirmedCallBack = onActionConfirmed;
            Owner = Application.Current.MainWindow;
            ShowDialog();
        }
        private void OnClickCancel(object sender, RoutedEventArgs e) => Close();

        private void OnClickConfirm(object sender, RoutedEventArgs e)
        {
            Close();
            _onConfirmedCallBack();
        }
    }
}
