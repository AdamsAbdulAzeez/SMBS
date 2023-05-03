using System;
using System.Collections.ObjectModel;
using System.Linq;
using AutoMapper;
using Prism.Events;
using Prism.Mvvm;
using Reactive.Bindings;
using WindowsClient.ApplicationLayout.LeftPane.DataExplorer.Commands;
using WindowsClient.Features.CreateModel;
using WindowsClient.Features.Transmissibility;
using WindowsClient.Services.Storage;
using WindowsClient.Shared.UIModels.MaterialBalance;
using WindowsClient.Shared.UserInteractions;

namespace WindowsClient.ApplicationLayout.LeftPane.DataExplorer
{
    internal class DataExplorerViewModel : BindableBase
    {
        public DataExplorerViewModel(IModelStore modelStore,
            IEventAggregator eventAggregator, 
            ITransmissibilityWindow transmissibilityWindow,
            Func<ICreateModelView> getCreateModelView,
            Func<IConfirmActionDialog> getConfirmationDialog,
            IMapper mapper,
            ToastNotification.IToastNotification toastNotification)      
        {
            modelStore.OnBusyChanged += isBusy => IsLoading.Value = isBusy;
            modelStore.ModelAdded += model => Models.Add(model);
            modelStore.ModelUpdated += OnModelUpdated ;
            OpenPvtMatchingTabCommand = new OpenPvtMatchingTabCommand(eventAggregator);
            OpenHistoryMatchingTabCommand = new OpenHistoryMatchingTabCommand(eventAggregator);
            OpenProductionPredictionTabCommand = new OpenProductionPredictionTabCommand(eventAggregator);
            OpenTankInputTabCommand = new OpenTankInputTabCommand(eventAggregator);
            OpenTransmissibilityWindowCommand = new OpenTransmissibilityWindowCommand(transmissibilityWindow);
            EditModelCommand = new EditModelCommand(getCreateModelView, mapper);
            SaveModelCommand = new SaveModelCommand(modelStore, toastNotification);

            modelStore.ModelDeleted += model => Models.Remove(model);
            OpenTransmissibilityWindowCommand = new OpenTransmissibilityWindowCommand(transmissibilityWindow);
            DeleteModelCommand = new DeleteModelCommand(modelStore, getConfirmationDialog);
        }

        private void OnModelUpdated(Model updatedModel)
        {
            var modelIndex = Models.IndexOf(Models.First(model => model.Id == updatedModel.Id));
            Models.Remove(Models[modelIndex]);
            Models.Insert(modelIndex, updatedModel);
        }

        public OpenPvtMatchingTabCommand OpenPvtMatchingTabCommand { get; }
        public OpenHistoryMatchingTabCommand OpenHistoryMatchingTabCommand { get; }
        public OpenProductionPredictionTabCommand OpenProductionPredictionTabCommand { get; }
        public OpenTankInputTabCommand OpenTankInputTabCommand { get; }
        public OpenTransmissibilityWindowCommand OpenTransmissibilityWindowCommand { get; }
        public DeleteModelCommand DeleteModelCommand { get; }
        public SaveModelCommand SaveModelCommand { get; }
        public EditModelCommand EditModelCommand { get; }
        public ReactiveProperty<bool> IsLoading { get; } = new(false);
        public ReactiveProperty<bool> ShowWatermark { get; } = new(true);
        public ObservableCollection<Model> Models { get; set; } = new();
    }
}
