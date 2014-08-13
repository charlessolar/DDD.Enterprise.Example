using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Presentation.Inventory
{
    public class Plugin : IPlugin
    {
        public void Register(IAppHost appHost)
        {
            appHost.RegisterService<SerialNumbers.SerialNumbers>("Serials");
            appHost.RegisterService<Items.Items>("Items");
        }
    }
}
