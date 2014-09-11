using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Presentation.Models.Cache;
using ServiceStack;

namespace Demo.Presentation.Models.Cache.Services
{
    [Route("/subscribe", "POST")]
    public class Subscribe
    {
        public String Domain { get; set; }

    }


    [Route("/unsubscribe", "POST")]
    public class Unsubscribe
    {
        public String Domain { get; set; }

    }
}
