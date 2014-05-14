using FluentValidation;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Validation
{
    public class StructureMapValidatorFactory : IValidatorFactory
    {
        private IContainer _container;
        private Dictionary<Type, Type> _validatorsByType;

        public StructureMapValidatorFactory(IContainer container)
        {
            _container = container;
            _validatorsByType = new Dictionary<Type, Type>();
            Populate();
        }

        public IValidator GetValidator(Type type)
        {
            if (!_validatorsByType.ContainsKey(type))
                return null;


            var instance = _container.GetInstance(_validatorsByType[type]) as IValidator;
            return instance;
        }

        public IValidator<T> GetValidator<T>()
        {
            return GetValidator(typeof(T)) as IValidator<T>;
        }

        private void Populate()
        {
            foreach (var v in AssemblyScanner.FindValidatorsInAssemblyContaining<IValidator>())
            {
                var genericArguments = v.ValidatorType.BaseType.GetGenericArguments();

                if (genericArguments.Length == 1)
                {
                    var targetType = genericArguments[0];
                    _validatorsByType[targetType] = v.ValidatorType;
                }
            }
        }
    }
}