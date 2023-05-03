using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using AutoMapper;
using Prism.Events;
using WindowsClient.ApplicationLayout.StatusBar;
using WindowsClient.Services.Storage.FileStorage.XmlModels.MaterialBalance;
using WindowsClient.Shared.ErrorHandling;
using WindowsClient.Shared.UIModels.MaterialBalance;

namespace WindowsClient.Services.Storage.FileStorage
{
    /// <summary>
    /// Implementation for storing models to flat file
    /// </summary>
    internal class FileStorageDataStore : IModelStore
    {
        public FileStorageDataStore(
          string defaultStoragePath,
          IEventAggregator eventAggregator,
          IMapper mapper)
        {
            _eventAggregator = eventAggregator;
            _mapper = mapper;

            _storagePath = defaultStoragePath;
            if (!Directory.Exists(_storagePath))
                Directory.CreateDirectory(_storagePath);
        }

        public Task LoadSavedFilesAsync()
        {
            OnBusyChanged?.Invoke(true);
            var progressBarEvent = _eventAggregator.GetEvent<ChangeStatusBarMessageEvent>();
            var xmlFiles = Directory
                .EnumerateFiles(_storagePath, "*.*", SearchOption.TopDirectoryOnly)
                .Where(s => "xml" == Path.GetExtension(s).TrimStart('.').ToLowerInvariant());
            foreach (var filePath in xmlFiles)
            {
                var fileName = Path.GetFileName(filePath).Split(".xml").First();
                progressBarEvent.Publish($"Loading {fileName}.");
                var savedModel = DeserializeMaterialBalanceModel(filePath);

                if (savedModel == null) continue;

                var uiModel = _mapper.Map<MaterialBalanceModel, Model>(savedModel);
                LoadedModels.Add(uiModel);
                ModelAdded?.Invoke(uiModel);
            }
            progressBarEvent.Publish(null);
            OnBusyChanged?.Invoke(false);
            return Task.CompletedTask;
        }
        private static MaterialBalanceModel DeserializeMaterialBalanceModel(string filePath)
        {
            using var reader = new StreamReader(filePath);
            var serializer = new XmlSerializer(typeof(MaterialBalanceModel));
            var model = serializer.Deserialize(reader) as MaterialBalanceModel;
            return model;
        }

        public async Task<IActionResult> SaveModelAsync(Model model)
        {
            try
            {
                var filePath = Path.Join(_storagePath, model.Name + ".xml");
                using var writter = new StreamWriter(filePath);
                var serializer = new XmlSerializer(typeof(MaterialBalanceModel));
                if (model.Id == Guid.Empty)
                {
                    model.Id = Guid.NewGuid(); 
                }

                var persistentModel = _mapper.Map<Model, MaterialBalanceModel>(model);
                serializer.Serialize(writter, persistentModel);
                LoadedModels.Add(model);
                ModelAdded?.Invoke(model);

                return await Task.FromResult(new SuccessfulAction());
            }
            catch (Exception e)
            {
                return new FailedResult(new List<string> { e.Message });
            }
        }

        public async Task<IActionResult> DeleteModelAsync(Model model)
        {
            try
            {
                var filePath = Path.Join(_storagePath, model.Name + ".xml");
                File.Delete(filePath);
                LoadedModels.Remove(model);
                ModelDeleted?.Invoke(model);

                return await Task.FromResult(new SuccessfulAction());
            }
            catch (Exception e)
            {
                return new FailedResult(new List<string> { e.Message });
            }
        }
        public async Task<IActionResult> UpdateModelAsync(Model model)
        {
            try
            {
                var oldModel = LoadedModels.FirstOrDefault(m => m.Id == model.Id);
                var oldFilePath = Path.Join(_storagePath, oldModel.Name + ".xml");
                File.Delete(oldFilePath);
                var filePath = Path.Join(_storagePath, model.Name + ".xml");
                await using var writter = new StreamWriter(filePath);
                var serializer = new XmlSerializer(typeof(MaterialBalanceModel));
                var persistentModel = _mapper.Map<Model, MaterialBalanceModel>(model);

                serializer.Serialize(writter, persistentModel);
                ModelUpdated?.Invoke(model);
                return await Task.FromResult(new SuccessfulAction());
            }
            catch (Exception e)
            {
                return new FailedResult(new List<string> { e.Message });
            }
        }


        public Task<IActionResult> UpdateTankAsync(Shared.UIModels.MaterialBalance.Tank tank)
        {
            var model = LoadedModels.FirstOrDefault(m => m.Id == tank.ModelId);
            return UpdateModelAsync(model);
        }

        public async Task<IActionResult> AddModelFromPath(string modelPath)
        {
            try
            {
                var fileStorageModel = DeserializeMaterialBalanceModel(modelPath);
                var uiModel = _mapper.Map<Model>(fileStorageModel);
                await SaveModelAsync(uiModel);
                return await Task.FromResult(new SuccessfulAction());
            }
            catch (Exception e)
            {
                return new FailedResult(new List<string> { e.Message });
            }
        }

        public async Task<IActionResult<Model>> GetModelAsync(Guid modelId)
        {
            var model = LoadedModels.FirstOrDefault(m => m.Id == modelId);

            if (model == null)
            {
                return await Task.FromResult
                    (new FailedResult<Model>(new List<string> { $"Model with {modelId} not found!" }));
            }
            return await Task.FromResult(new SuccessfulAction<Model>(model));
        }

        public event Action<bool> OnBusyChanged;
        public event Action<Model> ModelAdded;
        public event Action<Model> ModelUpdated;
        public event Action<Model> ModelDeleted;
        public IList<Model> LoadedModels { get; } = new List<Model>();
        public const string ProductionStoragePath = "C:/ProgramData/SMBS/Data/";
        private readonly IEventAggregator _eventAggregator;
        private readonly string _storagePath;
        private readonly IMapper _mapper;
    }
}
