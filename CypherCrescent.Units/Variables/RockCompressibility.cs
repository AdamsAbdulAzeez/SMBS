namespace CypherCrescent.Units.Variables
{
    /*
     * ===================================================================================
     * THIS IS AN AUTOGENERATED CLASS FROM THE CYPHERCRESCENT VARIABLE SYSTEM. 
     * CHANGES TO THIS FILE WILL BE OVERWRITTEN BY NEWER FILE GENERATIONS.
     * ===================================================================================
     */
    public sealed class RockCompressibility : VariableBase
    {
        public RockCompressibility()
        {
            Name = "RockCompressibility";
        }
        public RockCompressibility(double displayValue):base()
        {
            DatabaseValue = displayValue;
            Name = "RockCompressibility";
        }


        public static implicit operator RockCompressibility(double displayValue) => new RockCompressibility(displayValue);
        public static implicit operator double(RockCompressibility variable) => variable.DatabaseValue ?? 0;
    }
}
        