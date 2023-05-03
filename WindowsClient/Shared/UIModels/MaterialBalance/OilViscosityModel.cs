using System.ComponentModel;
using WindowsClient.Shared.EnumBinding;

namespace WindowsClient.Shared.UIModels.MaterialBalance
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum OilViscosityModel
    {
        [Description("Beal et al")]
        Beal_et_al,
        [Description("Beggs et al")]
        Beggs_et_al,
        [Description("Petrosky et al")]
        Petrosky_et_al,
        [Description("Egbogah et al")]
        Egbogah_et_al,
        [Description("Bergman Sutton")]
        Bergman_Sutton,
        [Description("De Ghetto")]
        DeGhetto,
    }
}
