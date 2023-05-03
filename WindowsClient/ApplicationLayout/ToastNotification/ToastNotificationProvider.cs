using System;
using System.Windows;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;
using ToastNotifications.Messages;

namespace WindowsClient.ApplicationLayout.ToastNotification
{
    internal class ToastNotificationProvider : IToastNotification
    {
        private Notifier notifier;

        public ToastNotificationProvider() => Configure();

        private void Configure()
        {
            notifier = new Notifier(cfg =>
            {
                cfg.PositionProvider = new WindowPositionProvider(
                    parentWindow: Application.Current.MainWindow,
                    corner: Corner.TopRight,
                    offsetX: 10,
                    offsetY: 10);

                cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                    notificationLifetime: TimeSpan.FromSeconds(3),
                    maximumNotificationCount: MaximumNotificationCount.FromCount(5));

                cfg.Dispatcher = Application.Current.Dispatcher;
            });
        }

        public void ShowError(string error, string title) => notifier.ShowError($"{title}:\n{error}");
        public void ShowInformation(string message, string title) => notifier.ShowInformation($"{title}:\n{message}");
    }
}
