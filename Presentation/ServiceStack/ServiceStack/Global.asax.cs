using System;

namespace Demo.Presentation.ServiceStack
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            new AppHost().Init();
        }
    }
}