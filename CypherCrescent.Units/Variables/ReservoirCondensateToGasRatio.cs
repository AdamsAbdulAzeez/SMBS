namespace CypherCrescent.Units.Variables
{
    /*
     * ===================================================================================
     * THIS IS AN AUTOGENERATED CLASS FROM THE CYPHERCRESCENT VARIABLE SYSTEM. 
     * CHANGES TO THIS FILE WILL BE OVERWRITTEN BY NEWER FILE GENERATIONS.
     * ===================================================================================
     */
    public sealed class ReservoirCondensateToGasRatio : VariableBase
    {
        public ReservoirCondensateToGasRatio()
        {
            Name = "ReservoirCondensateToGasRatio";
        }
        public ReservoirCondensateToGasRatio(double displayValue):base()
        {
            DatabaseValue = displayValue;
            Name = "ReservoirCondensateToGasRatio";
        }


        public static implicit operator ReservoirCondensateToGasRatio(double displayValue) => new ReservoirCondensateToGasRatio(displayValue);
        public static implicit operator double(ReservoirCondensateToGasRatio variable) => variable.DatabaseValue ?? 0;
    }
}
        