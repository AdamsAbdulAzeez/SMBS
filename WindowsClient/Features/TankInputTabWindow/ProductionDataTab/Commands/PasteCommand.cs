using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Windows;
using WindowsClient.ApplicationLayout.ToastNotification;
using WindowsClient.Shared.UIModels.MaterialBalance;

namespace WindowsClient.Features.TankInputTabWindow.ProductionDataTab.Commands
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

            if (parameter is not ProductionDataTabViewModel productionDataTabViewModel) return;

            try
            {
                string importText = CopiedText;
                importText = importText.Replace("\n", "");
                string[] lines = importText.Split('\r');
                string[] filteredLines = new string[lines.Length - 1];
                Array.Copy(lines, filteredLines, lines.Length - 1);
                var productionData = new List<ProductionDataRow>();

                foreach (var line in filteredLines)
                {
                    string[] tCells = line.Split("\t");
                    string[] cells = new string[7];
                    if (tCells.Length < 7)
                    {
                        Array.Copy(tCells, cells, tCells.Length);
                    }
                    else
                    {
                        cells = tCells;
                    }
                    DateTime time = DateTime.Parse(cells[0]);
                    var productionDataRow = new ProductionDataRow
                    {
                        Time = time,
                        Pressure = double.TryParse(cells[1], out _) ? double.Parse(cells[1]) : 0,
                        OilProduced = double.TryParse(cells[2], out _) ? double.Parse(cells[2]) : 0,
                        GasProduced = double.TryParse(cells[3], out _) ? double.Parse(cells[3]) : 0,
                        WaterProduced = double.TryParse(cells[4], out _) ? double.Parse(cells[4]) : 0,
                        GasInjected = double.TryParse(cells[5], out _) ? double.Parse(cells[5]) : 0,
                        WaterInjected = double.TryParse(cells[6], out _) ? double.Parse(cells[6]) : 0
                    };
                    productionData.Add(productionDataRow);
                }
                productionDataTabViewModel.Tank.ProductionData = productionData;
                productionDataTabViewModel.ProductionDataChanged?.Invoke();
            }
            catch (Exception e)
            {
                _toastNotification.ShowError("Copied table does not match the required format!", "Invalid paste action");
            }
        }
        
    }
}
