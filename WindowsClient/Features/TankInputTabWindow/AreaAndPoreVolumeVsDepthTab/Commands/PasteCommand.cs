using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Windows;
using WindowsClient.ApplicationLayout.ToastNotification;
using EngineShared = SMBS.Shared.DataImport;

namespace WindowsClient.Features.TankInputTabWindow.AreaAndPoreVolumeVsDepthTab.Commands
{
    internal class PasteCommand : DelegateCommandBase
    {
        private readonly AreaAndPoreVolumeVsDepthTabViewModel _viewModel;
        private readonly IToastNotification _toastNotification;

        public PasteCommand(AreaAndPoreVolumeVsDepthTabViewModel areaAndPoreVolumeVsDepthTabViewModel,
            IToastNotification toastNotification)
        {
            _viewModel = areaAndPoreVolumeVsDepthTabViewModel;
            _toastNotification = toastNotification;
        }
        protected override bool CanExecute(object parameter) => true;

        protected override void Execute(object parameter)
        {
            string copiedText = Clipboard.GetText();
            if (_viewModel.IsAvd)
                PasteAvd(copiedText);
            else
                PastePvd(copiedText);
        }

        private void PasteAvd(string CopiedText)
        {
            try
            {
                string importText = CopiedText;
                importText = importText.Replace("\n", "");
                string[] lines = importText.Split('\r');
                string[] filteredLines = new string[lines.Length - 1];
                Array.Copy(lines, filteredLines, lines.Length - 1);
                var areaDepthData = new List<EngineShared.AreaDepthRow>();

                foreach (var line in filteredLines)
                {
                    string[] tCells = line.Split("\t");
                    string[] cells = line.Split("\t");

                    var areaDepthRow = new EngineShared.AreaDepthRow
                    {
                        Depth = double.TryParse(cells[0], out _) ? double.Parse(cells[0]) : 0,
                        Area = double.TryParse(cells[1], out _) ? double.Parse(cells[1]) : 0,
                    };
                    areaDepthData.Add(areaDepthRow);
                }
                _viewModel.Tank.AreaDepthData.Clear();
                _viewModel.Tank.AreaDepthData.AddRange(areaDepthData);
                _viewModel.RaiseTankChanged();
            }
            catch (Exception)
            {
                _toastNotification.ShowError("Copied table does not match the required format!", "Invalid paste action");
            }
        }

        private void PastePvd(string CopiedText)
        {
            try
            {
                string importText = CopiedText;
                importText = importText.Replace("\n", "");
                string[] lines = importText.Split('\r');
                string[] filteredLines = new string[lines.Length - 1];
                Array.Copy(lines, filteredLines, lines.Length - 1);
                var poreVolumeDepthData = new List<EngineShared.PoreVolumeDepthRow>();

                foreach (var line in filteredLines)
                {
                    string[] tCells = line.Split("\t");
                    string[] cells = line.Split("\t");

                    var poreVolumeDepthRow = new EngineShared.PoreVolumeDepthRow
                    {
                        Depth = double.TryParse(cells[0], out _) ? double.Parse(cells[0]) : 0,
                        PoreVolume = double.TryParse(cells[1], out _) ? double.Parse(cells[1]) : 0,
                    };
                    poreVolumeDepthData.Add(poreVolumeDepthRow);
                }
                _viewModel.Tank.PoreVolumeDepth.Clear();
                _viewModel.Tank.PoreVolumeDepth.AddRange(poreVolumeDepthData);
                _viewModel.RaiseTankChanged();
            }
            catch (Exception)
            {
                _toastNotification.ShowError("Copied table does not match the required format!", "Invalid paste action");
            }
        }
    }
}
