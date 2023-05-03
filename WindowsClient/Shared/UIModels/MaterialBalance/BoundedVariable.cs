using CypherCrescent.Units.Variables;
using Prism.Mvvm;

namespace WindowsClient.Shared.UIModels.MaterialBalance
{
    internal static class BoundedVariableExtensions
    {
        public static RegressionVariable AsRegressionVariable<T>(this BoundedVariable<T> boundedVariable) where T: VariableBase, new()
        {
            return new RegressionVariable(
                boundedVariable.UpperBound.DisplayValue.Value,
                boundedVariable.LowerBound.DisplayValue.Value,
                boundedVariable.CurrentValue.DisplayValue.Value,
                boundedVariable.LowerBound.Name);
        }
    }

    public class BoundedVariable<T> : BindableBase where  T : VariableBase, new()
    {
        public BoundedVariable()
        {
            LowerBound = new();
            UpperBound = new();
            CurrentValue = new();
            Name = LowerBound.Name;

            LowerBound.DisplayValueChanged += (_) => CalculateCurrentValue();
            UpperBound.DisplayValueChanged += (_) => CalculateCurrentValue();
        }

        

        public void CalculateCurrentValue() => CurrentValue.DisplayValue = 0.5 * (LowerBound.DisplayValue + UpperBound.DisplayValue);

        public string Name { get; }
        public string VariableWithUnit => $"{Name}({LowerBound.DisplayUnit})";
        public T LowerBound { get; set; }
        public T UpperBound { get; set; }
        public T CurrentValue { get; set; }
    }
}
