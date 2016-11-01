using Demo.Presentation.ServiceStack.Infrastructure.Extensions;
using Demo.Library.Security;
using ServiceStack;
using ServiceStack.Web;
using StructureMap;
using System;
using System.Net;

namespace Demo.Presentation.ServiceStack.Infrastructure.Authentication
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class RequireJwtAttribute : RequestFilterAttribute
    {
        public IContainer Container { get; set; }

        public RequireJwtAttribute(ApplyTo applyTo = ApplyTo.All)
            : base(applyTo)
        {
            this.Priority = (int)RequestFilterPriority.Authenticate;
        }

        public override void Execute(IRequest req, IResponse res, object requestDto)
        {
            var requestType = requestDto.GetType();

            var profile = req.RetreiveUserProfile();

            if (profile == null)
            {
                res.StatusCode = (int)HttpStatusCode.Unauthorized;
                res.EndRequest();
                return;
            }

            var context = requestType.Namespace + "." + requestType.Name;

            var manager = this.Container.GetInstance<IManager>();
            while (true)
            {
                if (manager.Authorize(profile.UserId, context, "request"))
                    return;

                // Will throw exception when there are no more .'s
                context = context.SafeSubstring(0, context.LastIndexOf('.'));
            }
        }
    }
}