using Microsoft.Win32;
using Prism.Commands;
using System.Linq;
using WindowsClient.ApplicationLayout.ToastNotification;
using WindowsClient.Services.ExcelImport;

namespace WindowsClient.Features.TankInputTabWindow.ProductionDataTab.Commands
{
    internal class ImportProductionDataCommand : DelegateCommandBase
    {
        private readonly IMaterialBalanceDataReader _materialBalanceDataReader;
        private readonly IToastNotification _toastNotification;

        public ImportProductionDataCommand(IMaterialBalanceDataReader materialBalanceDataReader, 
            IToastNotification toastNotification)
        {
            _materialBalanceDataReader = materialBalanceDataReader;
            _toastNotification = toastNotification;
        }
        protected override bool CanExecute(object parameter) => true;

        protected override async void Execute(object parameter)
        {
            if (parameter is not ProductionDataTabViewModel productionDataTabViewModel) return;
            
            var readResult = await _materialBalanceDataReader.ReadProductionData();

            if (readResult.IsSuccess)
            {
                productionDataTabViewModel.Tank.ProductionData = readResult.Payload.ToList();
                productionDataTabViewModel.ProductionDataChanged?.Invoke();
                return;
            }

            if (readResult.Errors.Count > 1) 
                _toastNotification.ShowError("Failed to read production data from excel file.", "Operation failed");
        }
    }
}
