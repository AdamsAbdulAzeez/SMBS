namespace WindowsClient.Features
{
    public enum FeatureTabs
    {
        None,
        HistoryMatchingTehraniPlotTab,
        HistoryMatchingSimulationTab,
        HistoryMatchingAllPlotsTab,
        HistoryMatchingEnergyPlotTab,
        HistoryMatchingGraphicalPlotTab,
        HistoryMatchingWdPlotTab,
        HistoryMatchingFractionalFlowTab,
        PvtMatchingPvtInputTab,
        LabPvtTab,
        PvtMatchingExternalPvtDataTab,
        TankInputDataTab,
        TankInputProductionDataTab,
        TankInputWaterInfluxTab,
        TankInputRelPerm,
        TankInputAreaAndPoreVolumeVsDepth,
        CartesianDashboardTab,
        BarPlotDashboardTab,
        PythonScriptingTab,
        EneryPlotTab,
        GraphicalPlotTab,
        WDPlotTab,
        AllPlotsTab,
        PredictionSetupTab,
        ProductionAndConstraintsTab
    }

    public interface IFeatureWindowViewModel
    {
        void SubscribeToRibbonEvents();
        void UnsubscribeToRibbonEvents();
        void PublishActiveFeatureChanged();
    }
}
