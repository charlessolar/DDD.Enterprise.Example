using Demo.Library.Exceptions;
using Demo.Library.Security;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Extensibility;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.PostSharp
{
    [Serializable]
    public class AuthorizeAspect : TypeLevelAspect
    {
        [NonSerialized]
        private Func<IManager> _manager = () => ObjectFactory.GetInstance<IManager>();

        [OnMethodEntryAdvice, MulticastPointcut(Targets = MulticastTargets.Method, Attributes = MulticastAttributes.Public)]
        public void OnInvoke(MethodInterceptionArgs args)
        {
            var result = _manager().Authorize(args.Instance);
            if (!result.IsAuthorized)
                throw new SecurityException();
        }

        [OnLocationGetValueAdvice, MulticastPointcut(Targets = MulticastTargets.Property, Attributes = MulticastAttributes.Public)]
        public void OnGetValue(LocationInterceptionArgs args)
        {
            var result = _manager().Authorize(args.Instance);
            if (!result.IsAuthorized)
                throw new SecurityException();
        }

        [OnLocationGetValueAdvice, MulticastPointcut(Targets = MulticastTargets.Property, Attributes = MulticastAttributes.Public)]
        public void OnSetValue(LocationInterceptionArgs args)
        {
            var result = _manager().Authorize(args.Instance);
            if (!result.IsAuthorized)
                throw new SecurityException();
        }
    }
}