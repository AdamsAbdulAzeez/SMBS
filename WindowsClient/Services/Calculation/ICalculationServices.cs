using WindowsClient.Services.Calculation.FractionalFlowMatching;
using WindowsClient.Services.Calculation.PvtMatching;
using WindowsClient.Services.Calculator.HistoryMatching;
using WindowsClient.Services.Calculator.PressureSimulation;

namespace WindowsClient.Services.Calculation
{
    interface ICalculationServices
    {
        IHistoryMatchingService HistoryMatchingService { get;  }
        IPressureSimulationService PressureSimulationService { get;  }
        IPvtMatchingService PvtMatchingService { get; }
        IFractionalFlowMatchingService FractionalFlowMatchingService { get; }
    }
}
