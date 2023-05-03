using Prism.Events;
using Reactive.Bindings;
using System;

namespace WindowsClient.ApplicationLayout.StatusBar
{
    internal class StatusBarViewModel
    {
        public StatusBarViewModel(IEventAggregator eventAggregator)
        {
            eventAggregator
                .GetEvent<ChangeStatusBarMessageEvent>()
                .Subscribe(message => LoaderMessage.Value = message);

            LoaderMessage.Subscribe(message => IsLoading.Value = !string.IsNullOrEmpty(message));
        }

        public ReactiveProperty<string> LoaderMessage { get; set; } = new();
        public ReactiveProperty<bool> IsLoading { get; private set; } = new(false);
    }
}
