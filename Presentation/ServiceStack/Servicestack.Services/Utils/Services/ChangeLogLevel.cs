using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Presentation.ServiceStack.Utils.Services
{
    [Api("Utilities")]
    [Route("/util/logging", "POST")]
    public class ChangeLogLevel : IReturnVoid
    {
        public String LogLevel { get; set; }
    }
}
