using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Windows;
using WindowsClient.ApplicationLayout.ToastNotification;
using WindowsClient.Shared.UIModels.MaterialBalance;

namespace WindowsClient.Features.PvtMatchingTabWindow.LabPvtTableTab.Commands
{
    internal class PasteCommand : DelegateCommandBase
    {
        private readonly LabPvtTableTabViewModel _viewModel;
        private readonly IToastNotification _toastNotification;

        public PasteCommand(LabPvtTableTabViewModel labPvtTableTabViewModel, 
            IToastNotification toastNotification)
        {
            _viewModel = labPvtTableTabViewModel;
            _toastNotification = toastNotification;
        }
        protected override bool CanExecute(object parameter) => true;

        protected override void Execute(object parameter)
        {
            string CopiedText = Clipboard.GetText();
            try
            {
                string importText = CopiedText;
                importText = importText.Replace("\n", "");
                string[] lines = importText.Split('\r');
                string[] filteredLines = new string[lines.Length - 1];
                Array.Copy(lines, filteredLines, lines.Length - 1);
                var pvtMatchingInput = new List<PvtDataRow>();

                foreach (var line in filteredLines)
                {
                    string[] tCells = line.Split("\t");
                    string[] cells = new string[6];
                    if (tCells.Length < 6)
                    {
                        Array.Copy(tCells, cells, tCells.Length);
                    }
                    else
                    {
                        cells = tCells;
                    }
                    var pvtMatchingInputRow = new PvtDataRow
                    {
                        Pressure = double.TryParse(cells[0], out _) ?  double.Parse(cells[0]) : 0,
                        GasOilRatio = double.TryParse(cells[1], out _) ? double.Parse(cells[1]) : 0,
                        OilFVF = double.TryParse(cells[2], out _) ? double.Parse(cells[2]) : 0,
                        OilViscosity = double.TryParse(cells[3], out _) ? double.Parse(cells[3]) : 0,
                        GasFVF = double.TryParse(cells[4], out _) ? double.Parse(cells[4]) : 0,
                        GasViscosity = double.TryParse(cells[5], out _) ? double.Parse(cells[5]) : 0
                    };
                    pvtMatchingInput.Add(pvtMatchingInputRow);
                }
                _viewModel.Tank.SetPvtMatchingInput(pvtMatchingInput);
                _viewModel.Tank.PvtData = pvtMatchingInput;
            }
            catch (Exception)
            {
                _toastNotification.ShowError("Copied table does not match the required format!", "Invalid paste action");
            }
        }
    }
}
