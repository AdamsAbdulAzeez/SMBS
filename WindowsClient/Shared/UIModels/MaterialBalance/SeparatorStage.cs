using WindowsClient.Shared.EnumBinding;
using System.ComponentModel;

namespace WindowsClient.Shared.UIModels.MaterialBalance
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum SeparatorStage
    {
        [Description("Single Stage")]
        SingleStage = 0,
        [Description("Two Stage")]
        TwoStage = 1,
        [Description("Three Stage")]
        ThreeStage = 2
    }
}
