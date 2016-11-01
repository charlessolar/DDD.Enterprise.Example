using ServiceStack;

namespace Demo.Presentation.ServiceStack.Utils.Services
{
    [Api("Utilities")]
    [Route("/util/logging", "POST")]
    public class ChangeLogLevel  : Infrastructure.Commands.ServiceCommand
    {
        public string LogLevel { get; set; }
    }
}
