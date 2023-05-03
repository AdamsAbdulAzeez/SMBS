namespace CypherCrescent.Units.Variables
{
    /*
     * ===================================================================================
     * THIS IS AN AUTOGENERATED CLASS FROM THE CYPHERCRESCENT VARIABLE SYSTEM. 
     * CHANGES TO THIS FILE WILL BE OVERWRITTEN BY NEWER FILE GENERATIONS.
     * ===================================================================================
     */
    public sealed class DevelopedUltimateRecovery_Oil : VariableBase
    {
        public DevelopedUltimateRecovery_Oil()
        {
            Name = "DevelopedUltimateRecovery_Oil";
        }
        public DevelopedUltimateRecovery_Oil(double displayValue):base()
        {
            DatabaseValue = displayValue;
            Name = "DevelopedUltimateRecovery_Oil";
        }


        public static implicit operator DevelopedUltimateRecovery_Oil(double displayValue) => new DevelopedUltimateRecovery_Oil(displayValue);
        public static implicit operator double(DevelopedUltimateRecovery_Oil variable) => variable.DatabaseValue ?? 0;
    }
}
        