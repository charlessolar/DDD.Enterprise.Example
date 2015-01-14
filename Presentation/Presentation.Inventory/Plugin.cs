using ServiceStack;
using ServiceStack.Validation;

namespace Demo.Presentation.Inventory
{
    public class Plugin : IPlugin
    {
        public void Register(IAppHost appHost)
        {
            appHost.GetContainer().RegisterAutoWiredType(typeof(Items.Serials.Service));
            appHost.GetContainer().RegisterAutoWiredType(typeof(Items.Service));
            appHost.RegisterService<Items.Serials.Service>("/items/{ItemId}/serials");
            appHost.RegisterService<Items.Service>("/items");

            appHost.GetContainer().RegisterValidators(typeof(Plugin).Assembly);
        }
    }
}