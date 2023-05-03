using Prism.Commands;
using System.Linq;
using WindowsClient.ApplicationLayout.ToastNotification;
using WindowsClient.Services.ExcelImport;

namespace WindowsClient.Features.PvtMatchingTabWindow.ExternalPvtTableTab.Commands
{
    internal class ImportExternalPvtInputCommand : DelegateCommandBase
    {
        private readonly IMaterialBalanceDataReader _materialBalanceDataReader;
        private readonly IToastNotification _toastNotification;

        public ImportExternalPvtInputCommand(IMaterialBalanceDataReader materialBalanceDataReader, IToastNotification toastNotification)
        {
            _materialBalanceDataReader = materialBalanceDataReader;
            _toastNotification = toastNotification;
        }

        protected override bool CanExecute(object parameter) => true;

        protected override async void Execute(object parameter)
        {
            if (parameter is not ExternalPvtTableTabViewModel viewModel) return;

            var readResult = await _materialBalanceDataReader.ReadExternalPvtData(viewModel.Tank.FlowingFluid);

            if (readResult.IsSuccess)
            {
                viewModel.Tank.PvtData = readResult.Payload.ToList();
                viewModel.ExternalPvtChanged?.Invoke();
                return;
            }

            if (readResult.Errors.Count > 0)
                _toastNotification.ShowError("Failed to read External PVT data from excel file.", "Operation Failed!");

        } 
    }
}
