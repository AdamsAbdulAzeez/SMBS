using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CypherCrescent.Units.Contracts;
using CypherCrescent.Units.Annotations;

namespace CypherCrescent.Units.Variables
{
    public class VariableBase : INotifyPropertyChanged
    {
        private double? _displayValue;

        public string Name { get; protected internal set; }
        /// <summary>
        /// Collection of all unit option combinations
        /// </summary>
        public List<UnitSelectionOption> UnitOptions { get; internal set; }
        /// <summary>
        /// Defines the database value of this variable
        /// </summary>
        public double? DatabaseValue { get; set; }

        /// <summary>
        /// Defines the database value of this variable
        /// </summary>
        public double? DisplayValue
        {
            get => _displayValue;
            set
            {
                _displayValue = value;
                OnPropertyChanged(nameof(DisplayValue));
                DisplayValueChanged?.Invoke(value);
            }
        }

        /// <summary>
        /// Defines the display unit of the variable
        /// </summary>
        public UnitSelectionOption DisplayUnit { get; internal set; }

        /// <summary>
        /// Defines the database unit of the variable
        /// </summary>
        public UnitSelectionOption DatabaseUnit { get; internal set; }

        public event Action<double?> DisplayValueChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        public override string ToString() => $"{Name}: Database:{DatabaseValue} {DatabaseUnit} Display: {DisplayValue}{DisplayUnit}";

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public sealed class Dimensionless : VariableBase
    {
        public Dimensionless()
        {
            
        }
        public Dimensionless(double displayValue)
        {
            DatabaseValue = displayValue;
        }
        public new string Name { get; } = "NoUnit";
        public static implicit operator Dimensionless(double displayValue) => new Dimensionless(displayValue);
        public static implicit operator double(Dimensionless variable) => variable.DatabaseValue ?? 0;
    }
}
