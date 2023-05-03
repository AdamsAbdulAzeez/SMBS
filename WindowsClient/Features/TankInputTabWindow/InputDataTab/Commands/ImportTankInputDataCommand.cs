using Prism.Commands;
using WindowsClient.ApplicationLayout.ToastNotification;
using WindowsClient.Services.ExcelImport;

namespace WindowsClient.Features.TankInputTabWindow.InputDataTab.Commands
{
    internal class ImportTankInputDataCommand : DelegateCommandBase
    {
        private readonly IMaterialBalanceDataReader _materialBalanceDataReader;
        private readonly IToastNotification _toastNotification;
        private readonly InputDataTabViewModel _viewModel;

        public ImportTankInputDataCommand(InputDataTabViewModel viewModel,
            IMaterialBalanceDataReader materialBalanceDataReader, 
            IToastNotification toastNotification)
        {
            _viewModel = viewModel;
            _materialBalanceDataReader = materialBalanceDataReader;
            _toastNotification = toastNotification;
        }
        protected override bool CanExecute(object parameter) => true;

        protected override async void Execute(object parameter)
        {
            var readResult = await _materialBalanceDataReader.ReadHistoryMatchingInputData();

            if (readResult.IsSuccess)
            {
                _viewModel.Tank.StartOfProduction = readResult.Payload.StartOfProduction;
                _viewModel.Tank.FlowingFluid = readResult.Payload.FlowingFluid;
                _viewModel.Tank.InitialReservoirPressure = readResult.Payload.InitialReservoirPressure;
                _viewModel.Tank.ConnateWaterSaturation = readResult.Payload.ConnateWaterSaturation;

                _viewModel.Tank.GasCap = readResult.Payload.GasCap;
                _viewModel.Tank.STOIP = readResult.Payload.STOIP;
                _viewModel.Tank.GIIP = readResult.Payload.GIIP;
                _viewModel.Tank.Thickness = readResult.Payload.Thickness;
                _viewModel.Tank.Length = readResult.Payload.Length;
                _viewModel.Tank.Width = readResult.Payload.Width;
                _viewModel.Tank.Radius = readResult.Payload.Radius;

                _viewModel.Tank.SetTankAquifer(readResult.Payload.Aquifer);
                _viewModel.Tank.RelPermData = readResult.Payload.RelPermData;
                _viewModel.Tank.Rock = readResult.Payload.Rock;
                _viewModel.RefreshTankProperties();
                return;
            }

            if (readResult.Errors.Count > 1) 
                _toastNotification.ShowError("Failed to tank input data from excel file.", "Operation failed");
        }
    }
}
