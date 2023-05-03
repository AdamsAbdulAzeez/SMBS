using System.ComponentModel;
using WindowsClient.Shared.EnumBinding;

namespace WindowsClient.Shared.UIModels.MaterialBalance
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum RelativePermPlotType
    {
        [Description("Krw vs Sw")]
        KrwVsSw,
        [Description("Krg vs Sw")]
        KrgVsSw,
        [Description("Kro vs So")]
        KroVsSo,
        [Description("Krg vs Sg")]
        KrgVsSg,
        [Description("Kro vs Sw")]
        KroVsSw,
        [Description("Kro vs Sg")]
        KroVsSg
    }
}
