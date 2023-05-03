using Prism.Commands;
using WindowsClient.ApplicationLayout.ToastNotification;
using WindowsClient.Services.ExcelImport;

namespace WindowsClient.Features.PvtMatchingTabWindow.PvtInputTab.Commands
{
    internal class ImportPvtMatchingInputCommand : DelegateCommandBase
    {
        private readonly IMaterialBalanceDataReader _materialBalanceDataReader;
        private readonly IToastNotification _toastNotification;

        public ImportPvtMatchingInputCommand(IMaterialBalanceDataReader materialBalanceDataReader, 
            IToastNotification toastNotification)
        {
            _materialBalanceDataReader = materialBalanceDataReader;
            _toastNotification = toastNotification;
        }

        protected override bool CanExecute(object parameter) => true;
        protected override async void Execute(object parameter)
        {
            if (parameter is not PvtInputTabViewModel viewModel) return;

            var readResult = await _materialBalanceDataReader.ReadPvtMatchingInputData(viewModel.Tank.FlowingFluid);

            if (readResult.IsSuccess)
            {
                viewModel.Tank.SetPvtMatchingInput(readResult.Payload.PvtMatchingInput);
                viewModel.Tank.PvtInitialCondition = readResult.Payload.PvtInitialCondition;

                viewModel.RefreshTankProperties();
            }

            if (readResult.Errors.Count > 1)
                _toastNotification.ShowError("Failed to read PVT data from excel file.", "Operation failed");
        }
    }
}
