using WindowsClient.ApplicationLayout.ToastNotification;
using WindowsClient.Services.Storage;
using System;
using Prism.Mvvm;
using WindowsClient.Features.CreateModel;
using WindowsClient.Features.CreateModelWindow.Commands;
using WindowsClient.Shared.Features;
using WindowsClient.Shared.UIModels.MaterialBalance;
using WindowsClient.Shared.UserInteractions;

namespace WindowsClient.Features.CreateModelWindow
{
    internal class CreateModelViewModel : BindableBase
    {
        public CreateModelViewModel(ICreateModelView view,
            IModelStore modelStore,
            IToastNotification toastNotification,
            Func<IConfirmActionDialog> getConfirmationDialog)
        {
            View = view;
            SaveModelCommand = new SaveModelCommand(modelStore, toastNotification, this);
            UpdateModelCommand = new UpdateModelCommand(modelStore, toastNotification, this);
            AddTankCommand = new AddTankCommand(this);
            RemoveTankCommand = new RemoveTankCommand(this);
            Model.Tanks.CollectionChanged += (_, __) => RaiseModelCanSaveOrUpdate();
        }

        public ICreateModelView View { get; }
        public SaveModelCommand SaveModelCommand { get; set; }
        public UpdateModelCommand UpdateModelCommand { get; set; }
        public AddTankCommand AddTankCommand { get; set; }
        public RemoveTankCommand RemoveTankCommand { get; set; }
        public bool IsEditMode => DisplayMode == DisplayMode.Edit;
        public Model Model { get; set; } = new();
        public DisplayMode DisplayMode { get; set; }
        internal void RaiseModelCanSaveOrUpdate()
        {
            SaveModelCommand.RaiseCanExecuteChanged();
            UpdateModelCommand.RaiseCanExecuteChanged();
        }

        public bool IsLoading { get; set; }
        internal void SetIsLoading(bool isLoading) => IsLoading = isLoading;
    }
}
