using ServiceStack.Configuration;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.IoC
{
    public class StructureMapContainerAdapter : IContainerAdapter
    {
        public T TryResolve<T>()
        {
            return ObjectFactory.TryGetInstance<T>();
        }

        public T Resolve<T>()
        {
            return ObjectFactory.TryGetInstance<T>();
        }
    }
}