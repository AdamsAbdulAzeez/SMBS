using System.Windows;
using CypherCrescent.Units.Contracts;
using CypherCrescent.Units.Variables;

namespace CypherCrescent.Units.Controls
{
    /// <summary>
    /// Interaction logic for VariableTextBox.xaml
    /// </summary>
    public partial class VariableTextBox
    {
        public VariableTextBox()
        {
            InitializeComponent();
        }

        public VariableBase Variable
        {
            get => (VariableBase)GetValue(VariableProperty);
            set => SetValue(VariableProperty, value);
        }
        public static readonly DependencyProperty VariableProperty =
            DependencyProperty.Register("Variable", typeof(VariableBase), typeof(VariableTextBox), new PropertyMetadata(null));
    }
}
