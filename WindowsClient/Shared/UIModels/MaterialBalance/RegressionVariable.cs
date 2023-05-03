using Prism.Mvvm;

namespace WindowsClient.Shared.UIModels.MaterialBalance
{
    public class RegressionVariable : BindableBase
    {
        private RegressionVariable() { }
        public RegressionVariable(double upperBound, double lowerBound, double start, string name)
        {
            LowerBound = lowerBound;
            UpperBound = upperBound;
            Start = start;
            BestFitValue = start;
            Name = name;
            DisplayName = name;
        }

        public string Name { get; set; }
        private string displayName;
        public string DisplayName 
        { 
            get => displayName;
            set 
            { 
                displayName = value;
                RaisePropertyChanged();
            }
        }

        public bool ToBeOptimized { get; set; }
        private double _bestFitValue;
        public double BestFitValue
        {
            get => _bestFitValue;
            set { _bestFitValue = value; RaisePropertyChanged(nameof(BestFitValue)); }
        }

        private double _lowerBound;
        public double LowerBound
        {
            get => _lowerBound;
            set { _lowerBound = value; RaisePropertyChanged(nameof(LowerBound)); }
        }

        private double _start;
        public double Start
        {
            get => _start;
            set { _start = value; RaisePropertyChanged(nameof(Start)); }
        }

        private double _upperBound;
        public double UpperBound
        {
            get => _upperBound;
            set { _upperBound = value; RaisePropertyChanged(nameof(UpperBound)); }
        }
    }
}
