using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Windows;
using WindowsClient.ApplicationLayout.ToastNotification;
using WindowsClient.Shared.UIModels.MaterialBalance;

namespace WindowsClient.Features.PvtMatchingTabWindow.PvtInputTab.Commands
{
    internal class PasteCommand : DelegateCommandBase
    {
        private readonly IToastNotification _toastNotification;

        public PasteCommand(IToastNotification toastNotification)
        {
            _toastNotification = toastNotification;
        }
        protected override bool CanExecute(object parameter) => true;

        protected override void Execute(object parameter)
        {
            string CopiedText = Clipboard.GetText();

            if (parameter is not PvtInputTabViewModel pvtInputTabViewModel) return;

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
                    string[] cells = line.Split("\t");
                    var pvtMatchingInputRow = new PvtDataRow
                    {
                        Pressure = double.Parse(cells[0]),
                        GasOilRatio = double.Parse(cells[1]),
                        OilFVF = double.Parse(cells[2]),
                        OilViscosity = double.Parse(cells[3]),
                        GasFVF = double.Parse(cells[4]),
                        GasViscosity = double.Parse(cells[5])
                    };
                    pvtMatchingInput.Add(pvtMatchingInputRow);
                }
                pvtInputTabViewModel.Tank.SetPvtMatchingInput(pvtMatchingInput);
                pvtInputTabViewModel.PvtMatchingInputChanged?.Invoke();
            }
            catch (Exception)
            {
                _toastNotification.ShowError("Copied table does not match the required format!", "Invalid paste action");
            }
        }
    }
}
