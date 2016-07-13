using ServiceStack;
using ServiceStack.Validation;

namespace Demo.Presentation.ServiceStack.Authentication
{
    public class Plugin : IPlugin
    {
        public void Register(IAppHost appHost)
        {
            appHost.RegisterService<Users.Service>();
            appHost.GetContainer().RegisterAutoWiredType(typeof(Users.Service));

            //appHost.GetContainer().Register<Users.Service>((c) => new Users.Service(c.Resolve<IBus>()));
            appHost.GetContainer().RegisterValidators(typeof(Plugin).Assembly);
        }
    }
}