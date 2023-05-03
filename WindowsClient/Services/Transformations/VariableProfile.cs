using AutoMapper;
using CypherCrescent.Units.Services;
using CypherCrescent.Units.Variables;
using WindowsClient.Services.Storage.FileStorage.XmlModels.MaterialBalance;
using SMBS.Shared;
using WindowsClient.Shared.UIModels.MaterialBalance;

namespace WindowsClient.Services.Storage.Transformations
{
    public class BoundedVariableProfile : Profile
    {
        public BoundedVariableProfile()
        {
            CreateMap(typeof(BoundedVariable), typeof(BoundedVariable<>))
                .ConvertUsing(typeof(VariableConverter<>));

            CreateMap(typeof(BoundedVariable<>), typeof(BoundedVariable))
                .ConvertUsing(typeof(VariableConverter<>));

            CreateMap(typeof(VariableBase), typeof(double))
                .ConvertUsing(typeof(VariableToDoubleConverter));

            CreateMap(typeof(BoundedVariable<>), typeof(Variable))
                .ConvertUsing(typeof(EngineVariableConverter<>));
        }

        public class VariableConverter<TVariable> : 
            ITypeConverter<BoundedVariable, BoundedVariable<TVariable>>,
            ITypeConverter<BoundedVariable<TVariable>, BoundedVariable> where TVariable : VariableBase, new()
        {
            public VariableConverter(IUnitService unitService)
            {
                _unitService = unitService;
            }
            public BoundedVariable<TVariable> Convert(BoundedVariable source, BoundedVariable<TVariable> destination, ResolutionContext context)
            {
                if (source == null) return null;
                if (destination == null) destination = new BoundedVariable<TVariable>();
                destination.CurrentValue.DatabaseValue = source.CurrentValue;
                destination.LowerBound.DatabaseValue = source.LowerBound;
                destination.UpperBound.DatabaseValue = source.UpperBound;

                _unitService.RegisterVariable(destination.LowerBound);
                _unitService.RegisterVariable(destination.UpperBound);
                _unitService.RegisterVariable(destination.CurrentValue);

                return destination;
            }

            public BoundedVariable Convert(BoundedVariable<TVariable> source, BoundedVariable destination, ResolutionContext context)
            {
                if (source == null) return null;

                if (destination == null) destination = new BoundedVariable();
                destination.LowerBound = source.LowerBound.DisplayValue ?? 0;
                destination.UpperBound = source.UpperBound.DisplayValue ?? 0;
                destination.CurrentValue = source.CurrentValue.DisplayValue ?? 0;

                return destination;
            }

            private readonly IUnitService _unitService;
        }

        public class EngineVariableConverter<TVariable> :
            ITypeConverter<Variable, BoundedVariable<TVariable>>,
            ITypeConverter<BoundedVariable<TVariable>, Variable> where TVariable : VariableBase, new()
        {
            public EngineVariableConverter(IUnitService unitService)
            {
                _unitService = unitService;
            }

            public BoundedVariable Convert(BoundedVariable<TVariable> source, BoundedVariable destination, ResolutionContext context)
            {
                if (source == null) return null;

                if (destination == null) destination = new BoundedVariable();
                destination.LowerBound = source.LowerBound.DisplayValue ?? 0;
                destination.UpperBound = source.UpperBound.DisplayValue ?? 0;
                destination.CurrentValue = source.CurrentValue.DisplayValue ?? 0;

                return destination;
            }

            public Variable Convert(BoundedVariable<TVariable> source, Variable destination, ResolutionContext context)
            {
                if (source == null) return null;

                if (destination == null) destination = new Variable();
                destination.LowerBound = source.LowerBound.DisplayValue ?? 0;
                destination.UpperBound = source.UpperBound.DisplayValue ?? 0;
                destination.CurrentValue = source.CurrentValue.DisplayValue ?? 0;

                return destination;
            }

            public BoundedVariable<TVariable> Convert(Variable source, BoundedVariable<TVariable> destination, ResolutionContext context)
            {
                if (source == null) return null;
                if (destination == null) destination = new BoundedVariable<TVariable>();
                destination.CurrentValue.DisplayValue = source.CurrentValue;
                destination.LowerBound.DatabaseValue = source.LowerBound;
                destination.UpperBound.DatabaseValue = source.UpperBound;

                _unitService.RegisterVariable(destination.LowerBound);
                _unitService.RegisterVariable(destination.UpperBound);
                _unitService.RegisterVariable(destination.CurrentValue);

                return destination;
            }

            private readonly IUnitService _unitService;
        }

        public class VariableToDoubleConverter :
            ITypeConverter<VariableBase, double>,
            ITypeConverter<double, VariableBase>
        {
            public VariableToDoubleConverter(IUnitService unitService)
            {
                _unitService = unitService;
            }

            public double Convert(VariableBase source, double destination, ResolutionContext context)
            {
                if (source.DisplayValue == null) source.DisplayValue = 0;
                destination = source.DisplayValue.Value;
                return destination;
            }

            public VariableBase Convert(double source, VariableBase destination, ResolutionContext context)
            {
                if (destination == null) destination = new VariableBase();
                destination.DisplayValue = source;
                _unitService.RegisterVariable(destination);

                return destination;
            }

            private readonly IUnitService _unitService;
        }
    }
}
