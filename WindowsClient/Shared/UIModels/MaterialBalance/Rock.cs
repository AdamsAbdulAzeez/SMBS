using CypherCrescent.Units.Variables;
using Prism.Mvvm;

namespace WindowsClient.Shared.UIModels.MaterialBalance
{
    public class Rock : BindableBase
    {
        public Rock()
        {
            Porosity = new BoundedVariable<Porosity>();
            Permeability = new BoundedVariable<ReservoirPermeability>();
            Anisotropy = new BoundedVariable<Dimensionless>();
        }

        public BoundedVariable<ReservoirPermeability> Permeability { get; set; }
        public BoundedVariable<Dimensionless> Anisotropy { get; set; }
        public BoundedVariable<Porosity> Porosity { get; set; }
        public double Compressibility { get; set; }
    }
}
