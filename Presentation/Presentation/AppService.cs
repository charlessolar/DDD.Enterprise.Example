using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Demo.Presentation
{
    [ClientCanSwapTemplates]
    [DefaultView("App")]
    public class AppService : Service
    {
        public Object Get()
        {
            return null;
        }
    }
}