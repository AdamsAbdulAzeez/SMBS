using System.ComponentModel;
using WindowsClient.Shared.EnumBinding;

namespace WindowsClient.Shared.UIModels.MaterialBalance
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum HistoryVariableType
    {
        [Description("Tank Pressure")]
        Pressure,
        [Description("Cummulative Oil Production")]
        CummulativeOilProduction,
        [Description("Cummulative Gas Production")]
        CummulativeGasProduction,
        [Description("Cummulative Water Injection")]
        CummulativeWaterInjection,
        [Description("Cummulative Gas Injection")]
        CummulativeGasInjection,
        [Description("Cummulative Water Production")]
        CummulativeWaterProduction,
    }
}
