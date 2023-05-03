using System.ComponentModel;
using WindowsClient.Shared.EnumBinding;

namespace WindowsClient.Shared.UIModels.MaterialBalance
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum SimulationVariableType
    {
        [Description("Tank Pressure")]
        Pressure,
        [Description("Aquifer Influx")]
        AquiferInflux,
        [Description("Average Gas Injection Rate")]
        AverageGasInjectionRate,
        [Description("Average Gas Rate")]
        AverageGasRate,
        [Description("Average Liquid Rate")]
        AverageLiquidRate,
        [Description("Average Oil Rate")]
        AverageOilRate,
        [Description("Average Water Injection Rate")]
        AverageWaterInjectionRate,
        [Description("Average Water Rate")]
        AverageWaterRate,
        [Description("Cummulative Gas Injection")]
        CummulativeGasInjection,
        [Description("Cummulative Gas Production")]
        CummulativeGasProduction,
        [Description("Cummulative Oil Production")]
        CummulativeOilProduction,
        [Description("Cummulative Water Injection")]
        CummulativeWaterInjection,
        [Description("Oil Density")]
        OilDensity,
        [Description("Oil FVF")]
        OilFVF,
        [Description("Oil Recovery Factor")]
        OilRecoveryFactor,
        [Description("Oil Viscosity")]
        OilViscosity,
        [Description("Producing CGR")]
        ProducingCGR,
        [Description("Producing GOR")]
        ProducingGOR,
        [Description("Oil Water Contact")]
        OilWaterContact,
        [Description("Gas Oil Contact")]
        GasOilContact,
    }
}
