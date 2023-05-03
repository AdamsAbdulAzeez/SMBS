using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsClient.Shared.UIModels.MaterialBalance
{
    public class SimulationVariableModelType : BindableBase
    {
        public SimulationVariableType VariableName { get; set; }

        private bool isEnabled;
        public bool IsEnabled
        {
            get => isEnabled;
            set
            {
                isEnabled = value;
                RaisePropertyChanged();
            }
        }
    }
}
