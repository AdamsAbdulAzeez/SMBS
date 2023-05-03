using WindowsClient.Services.Calculation.FractionalFlowMatching;
using WindowsClient.Services.Calculation.PvtMatching;
using WindowsClient.Services.Calculator.HistoryMatching;
using WindowsClient.Services.Calculator.PressureSimulation;

namespace WindowsClient.Services.Calculation
{
    internal class CalculationServices : ICalculationServices
    {
        public CalculationServices(IHistoryMatchingService historyMatchingSerive, 
            IPressureSimulationService pressureSimulationService, 
            IPvtMatchingService pvtMatchingService, 
            IFractionalFlowMatchingService fractionalFlowMatchingService)
        {
            HistoryMatchingService = historyMatchingSerive;
            PressureSimulationService = pressureSimulationService;
            PvtMatchingService = pvtMatchingService;
            FractionalFlowMatchingService = fractionalFlowMatchingService;
        }
        public IHistoryMatchingService HistoryMatchingService { get; }
        public IPressureSimulationService PressureSimulationService { get; }
        public IPvtMatchingService PvtMatchingService { get; }
        public IFractionalFlowMatchingService FractionalFlowMatchingService { get; }
    }
}
