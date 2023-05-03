using System.ComponentModel;
using WindowsClient.Shared.EnumBinding;

namespace WindowsClient.Shared.UIModels.MaterialBalance
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum PbRsBoModel
    {
        Glaso,
        Standing,
        Lasater,
        [Description("Vazquez Beggs")]
        Vazquez_Beggs,
        [Description("Petrosky et al")]
        Petrosky_et_al,
        [Description("Al Marhoun")]
        Al_Marhoun,
        [Description("De Ghetto")]
        DeGhetto,
    }
}
