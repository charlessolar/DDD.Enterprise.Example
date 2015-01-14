using Castle.DynamicProxy;
using Demo.Library.Exceptions;
using Demo.Library.Extensions;
using StructureMap;
using System;

namespace Demo.Library.Security
{
    public class SecurityInterceptor : IInterceptor
    {
        private readonly IContainer _container;
        private readonly IManager _manager;

        public SecurityInterceptor(IContainer container, IManager manager)
        {
            _container = container;
            _manager = manager;
        }

        public void Intercept(IInvocation invocation)
        {
            var auth = (String)_container.GetInstance(typeof(String), "Authorization-Token");

            if (invocation.Method.IsSpecialName && invocation.Method.Name.StartsWith("get_"))
                if (!_manager.Authorize(auth, invocation.Method.GetSecurityContext(), "read"))
                    throw new SecurityException();

            if (invocation.Method.IsSpecialName && invocation.Method.Name.StartsWith("set_"))
                if (!_manager.Authorize(auth, invocation.Method.GetSecurityContext(), "write"))
                    throw new SecurityException();

            invocation.Proceed();
        }
    }
}