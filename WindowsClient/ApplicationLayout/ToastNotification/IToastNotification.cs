namespace WindowsClient.ApplicationLayout.ToastNotification
{
    internal interface IToastNotification
    {
        void ShowError(string error, string title);
        void ShowInformation(string message, string title);
    }
}