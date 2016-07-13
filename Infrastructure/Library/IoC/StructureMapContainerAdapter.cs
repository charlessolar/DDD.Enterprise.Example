using ServiceStack.Configuration;
using StructureMap;
using System;

namespace Demo.Library.IoC
{
    public class StructureMapContainerAdapter : IContainerAdapter
    {
        private readonly IContainer _container;

        public StructureMapContainerAdapter(IContainer container)
        {
            _container = container;
        }

        public T TryResolve<T>()
        {
            return _container.TryGetInstance<T>();
        }

        public T Resolve<T>()
        {
            var ret = _container.TryGetInstance<T>();
            if (ret == null) throw new ArgumentException(String.Format("Unknown resolution type '{0}'", typeof(T)));
            return ret;
        }
    }
}