namespace CypherCrescent.Units.Variables
{
    /*
     * ===================================================================================
     * THIS IS AN AUTOGENERATED CLASS FROM THE CYPHERCRESCENT VARIABLE SYSTEM. 
     * CHANGES TO THIS FILE WILL BE OVERWRITTEN BY NEWER FILE GENERATIONS.
     * ===================================================================================
     */
    public sealed class WaterRate : VariableBase
    {
        public WaterRate()
        {
            Name = "WaterRate";
        }
        public WaterRate(double displayValue):base()
        {
            DatabaseValue = displayValue;
            Name = "WaterRate";
        }


        public static implicit operator WaterRate(double displayValue) => new WaterRate(displayValue);
        public static implicit operator double(WaterRate variable) => variable.DatabaseValue ?? 0;
    }
}
        