namespace CypherCrescent.Units.Variables
{
    /*
     * ===================================================================================
     * THIS IS AN AUTOGENERATED CLASS FROM THE CYPHERCRESCENT VARIABLE SYSTEM. 
     * CHANGES TO THIS FILE WILL BE OVERWRITTEN BY NEWER FILE GENERATIONS.
     * ===================================================================================
     */
    public sealed class WaterSalinity : VariableBase
    {
        public WaterSalinity()
        {
            Name = "WaterSalinity";
        }
        public WaterSalinity(double displayValue):base()
        {
            DatabaseValue = displayValue;
            Name = "WaterSalinity";
        }


        public static implicit operator WaterSalinity(double displayValue) => new WaterSalinity(displayValue);
        public static implicit operator double(WaterSalinity variable) => variable.DatabaseValue ?? 0;
    }
}
        