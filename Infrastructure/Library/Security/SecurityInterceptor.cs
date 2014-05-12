using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Security
{
    public interface ISecurityInterceptor : IInterceptor
    {
    }

    public class SecurityInterceptor : ISecurityInterceptor
    {
        private readonly IManager _manager;

        public SecurityInterceptor(IManager manager)
        {
            _manager = manager;
        }
        public void Intercept(IInvocation invocation)
        {
            var result = _manager.Authorize<Actions.ExecutingAction>(invocation);
            if (result.IsAuthorized)
                invocation.Proceed();

            //// If is property get?
            //var result = _manager.Authorize<Actions.ReadingAction>(invocation);
            //if (result.IsAuthorized)
            //    invocation.Proceed();

            //// If is property set?
            //var result = _manager.Authorize<Actions.WritingAction>(invocation);
            //if (result.IsAuthorized)
            //    invocation.Proceed();
        }
    }
}