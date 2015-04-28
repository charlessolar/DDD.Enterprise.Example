using ServiceStack;
using ServiceStack.Validation;

namespace Demo.Application.ServiceStack.Configuration
{
    public class Plugin : IPlugin
    {
        public void Register(IAppHost appHost)
        {
            appHost.RegisterService<AccountType.Service>();
            appHost.RegisterService<TaxType.Service>();
            appHost.RegisterService<Region.Service>();
            appHost.RegisterService<Region.Entities.Match.Service>();
            appHost.RegisterService<Country.Service>();
            appHost.RegisterService<PaymentMethod.Service>();
            appHost.RegisterService<PaymentMethod.Entities.Detail.Service>();

            //appHost.GetContainer().RegisterAutoWiredType(typeof(Users.Service));

            //appHost.GetContainer().Register<Users.Service>((c) => new Users.Service(c.Resolve<IBus>()));
            appHost.GetContainer().RegisterValidators(typeof(Plugin).Assembly);
        }
    }
}