using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using WindowsClient.ApplicationLayout.ToastNotification;
using WindowsClient.Shared.UIModels.MaterialBalance;

namespace WindowsClient.Features.PvtMatchingTabWindow.ExternalPvtTableTab.Commands
{
    internal class PasteCommand : DelegateCommandBase
    {
        private readonly IToastNotification _toastNotification;

        public PasteCommand(IToastNotification toastNotification)
        {
            _toastNotification = toastNotification;
        }
        protected override bool CanExecute(object parameter) => true;

        protected override async void Execute(object parameter)
        {
            string CopiedText = Clipboard.GetText();

            if (parameter is not ExternalPvtTableTabViewModel viewModel) return;

            try
            {
                string importText = CopiedText;
                importText = importText.Replace("\n", "");
                string[] lines = importText.Split('\r');
                string[] filteredLines = new string[lines.Length - 1];
                Array.Copy(lines, filteredLines, lines.Length - 1);
                var pvtMatchingInput = new List<PvtDataRow>();
                switch (viewModel.Tank.FlowingFluid)
                {
                    case FluidType.Oil:
                        await PasteExternalPvtForOil(filteredLines, pvtMatchingInput);
                        break;
                    case FluidType.Gas:
                        await PasteExternalPvtForGas(filteredLines, pvtMatchingInput);
                        break;
                    case FluidType.Condensate:
                        await PasteExternalPvtForCondensate(filteredLines, pvtMatchingInput);
                        break;
                    default:
                        break;
                }              
                viewModel.Tank.PvtInitialCondition.BubblePoint = pvtMatchingInput[0].BubblePoint;
                viewModel.Tank.PvtInitialCondition.Temperature = pvtMatchingInput[0].Temperature;
                viewModel.Tank.PvtData = pvtMatchingInput;
                viewModel.ExternalPvtChanged?.Invoke();
            }
            catch (Exception)
            {
                _toastNotification.ShowError("Copied table does not match the required format!", "Invalid paste action");
            }

            
        }

        private static async Task PasteExternalPvtForOil(string[] filteredLines, List<PvtDataRow> pvtMatchingInput)
        {
            await Task.Run(() =>
            {
                foreach (var line in filteredLines)
                {
                    string[] cells = line.Split("\t");
                    var pvtMatchingInputRow = new PvtDataRow
                    {
                        Temperature = double.TryParse(cells[0], out _) ? double.Parse(cells[0]) : 0,
                        Pressure = double.TryParse(cells[1], out _) ? double.Parse(cells[1]) : 0,
                        BubblePoint = double.TryParse(cells[2], out _) ? double.Parse(cells[2]) : 0,
                        GasOilRatio = double.TryParse(cells[3], out _) ? double.Parse(cells[3]) : 0,
                        OilFVF = double.TryParse(cells[4], out _) ? double.Parse(cells[4]) : 0,
                        OilViscosity = double.TryParse(cells[5], out _) ? double.Parse(cells[5]) : 0,
                        ZFactor = double.TryParse(cells[6], out _) ? double.Parse(cells[6]) : 0,
                        GasFVF = double.TryParse(cells[7], out _) ? double.Parse(cells[7]) : 0,
                        GasViscosity = double.TryParse(cells[8], out _) ? double.Parse(cells[8]) : 0,
                        OilDensity = double.TryParse(cells[9], out _) ? double.Parse(cells[9]) : 0,
                        GasDensity = double.TryParse(cells[10], out _) ? double.Parse(cells[10]) : 0,
                        WaterFVF = double.TryParse(cells[11], out _) ? double.Parse(cells[11]) : 0,
                        WaterViscosity = double.TryParse(cells[12], out _) ? double.Parse(cells[12]) : 0,
                        WaterDensity = double.TryParse(cells[13], out _) ? double.Parse(cells[13]) : 0,
                        WaterCompressibility = double.TryParse(cells[14], out _) ? double.Parse(cells[14]) : 0,
                    };
                    pvtMatchingInput.Add(pvtMatchingInputRow);
                }
            });
        }
        private static async Task PasteExternalPvtForGas(string[] filteredLines, List<PvtDataRow> pvtMatchingInput)
        {
            await Task.Run(() => {
            foreach (var line in filteredLines)
            {
                string[] cells = line.Split("\t");
                double val = 0;
                var pvtMatchingInputRow = new PvtDataRow
                {
                    Temperature = double.TryParse(cells[0], out val) ? val : 0,
                    Pressure = double.TryParse(cells[1], out val) ? val : 0,
                    BubblePoint = double.TryParse(cells[2], out val) ? val : 0,
                    ZFactor = double.TryParse(cells[3], out val) ? val : 0,
                    GasFVF = double.TryParse(cells[4], out val) ? val : 0,
                    GasViscosity = double.TryParse(cells[5], out val) ? val : 0,
                    GasDensity = double.TryParse(cells[6], out val) ? val : 0,
                    PseudoPressure = double.TryParse(cells[7], out val) ? val : 0,
                    WaterFVF = double.TryParse(cells[8], out val) ? val : 0,
                    WaterViscosity = double.TryParse(cells[9], out val) ? val : 0,
                    WaterDensity = double.TryParse(cells[10], out val) ? val : 0,
                    WaterCompressibility = double.TryParse(cells[11], out val) ? val : 0,
                };
                pvtMatchingInput.Add(pvtMatchingInputRow);
            }
            });
           
        }

        private static async Task PasteExternalPvtForCondensate(string[] filteredLines, List<PvtDataRow> pvtMatchingInput)
        {
            await Task.Run(() => {

                foreach (var line in filteredLines)
                {
                    string[] cells = line.Split("\t");
                    double val = 0;
                    var pvtMatchingInputRow = new PvtDataRow
                    {
                        Temperature = double.TryParse(cells[0], out val) ? val : 0,
                        Pressure = double.TryParse(cells[1], out val) ? val : 0,
                        DewPoint = double.TryParse(cells[2], out val) ? val : 0,
                        ReservoirCGR = double.TryParse(cells[3], out val) ? val : 0,
                        ZFactor = double.TryParse(cells[4], out val) ? val : 0,
                        GasFVF = double.TryParse(cells[5], out val) ? val : 0,
                        GasViscosity = double.TryParse(cells[6], out val) ? val : 0,
                        GasDensity = double.TryParse(cells[7], out val) ? val : 0,
                        PseudoPressure = double.TryParse(cells[8], out val) ? val : 0,
                        OilFVF = double.TryParse(cells[9], out val) ? val : 0,
                        OilViscosity = double.TryParse(cells[10], out val) ? val : 0,
                        OilDensity = double.TryParse(cells[11], out val) ? val : 0,
                        WaterFVF = double.TryParse(cells[12], out val) ? val : 0,
                        WaterViscosity = double.TryParse(cells[13], out val) ? val : 0,
                        WaterDensity = double.TryParse(cells[14], out val) ? val : 0,
                        WaterCompressibility = double.TryParse(cells[15], out val) ? val : 0,
                        VaporizedCGR = double.TryParse(cells[16], out val) ? val : 0,
                        VapourisedWGR = double.TryParse(cells[17], out val) ? val : 0,
                    };
                    pvtMatchingInput.Add(pvtMatchingInputRow);
                }
            });
        }
    }
}
