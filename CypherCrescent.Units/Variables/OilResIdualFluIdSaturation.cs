namespace CypherCrescent.Units.Variables
{
    /*
     * ===================================================================================
     * THIS IS AN AUTOGENERATED CLASS FROM THE CYPHERCRESCENT VARIABLE SYSTEM. 
     * CHANGES TO THIS FILE WILL BE OVERWRITTEN BY NEWER FILE GENERATIONS.
     * ===================================================================================
     */
    public sealed class OilResIdualFluIdSaturation : VariableBase
    {
        public OilResIdualFluIdSaturation()
        {
            Name = "OilResIdualFluIdSaturation";
        }
        public OilResIdualFluIdSaturation(double displayValue):base()
        {
            DatabaseValue = displayValue;
            Name = "OilResIdualFluIdSaturation";
        }


        public static implicit operator OilResIdualFluIdSaturation(double displayValue) => new OilResIdualFluIdSaturation(displayValue);
        public static implicit operator double(OilResIdualFluIdSaturation variable) => variable.DatabaseValue ?? 0;
    }
}
        