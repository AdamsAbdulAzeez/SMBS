using Prism.Commands;

namespace WindowsClient.Features.TankInputTabWindow.AreaAndPoreVolumeVsDepthTab.Commands
{
    internal class ClearCommand : DelegateCommandBase
    {
        private readonly AreaAndPoreVolumeVsDepthTabViewModel _viewModel;

        public ClearCommand(AreaAndPoreVolumeVsDepthTabViewModel areaAndPoreVolumeVsDepthTabViewModel)
        {
            _viewModel = areaAndPoreVolumeVsDepthTabViewModel;
        }
        protected override bool CanExecute(object parameter) => true;

        protected override void Execute(object parameter)
        {
            if (_viewModel.IsPvd)
                _viewModel.Tank.PoreVolumeDepth.Clear();
            else
                _viewModel.Tank.AreaDepthData.Clear();
            _viewModel.RaiseTankChanged();
        }
    }
}
