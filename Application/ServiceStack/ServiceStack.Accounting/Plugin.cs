using System.Linq;
using Demo.Library.Queries;
using ServiceStack;
using ServiceStack.Validation;

namespace Demo.Application.ServiceStack.Accounting
{
    public class Plugin : IPlugin
    {
        public void Register(IAppHost appHost)
        {
            appHost.RegisterService<Account.Service>();
            appHost.RegisterService<Currency.Service>();

            //appHost.GetContainer().RegisterAutoWiredType(typeof(Users.Service));

            //var container = appHost.GetContainer().Resolve<SimpleInjector.Container>();
            //container.RegisterManyForOpenGeneric(typeof(IQueryHandler<,>), typeof(Plugin).Assembly);
            //container.RegisterManyForOpenGeneric(typeof(IPagingQueryHandler<,>), typeof(Plugin).Assembly);

            //appHost.GetContainer().Register<Users.Service>((c) => new Users.Service(c.Resolve<IBus>()));
            appHost.GetContainer().RegisterValidators(typeof(Plugin).Assembly);
        }
    }
}